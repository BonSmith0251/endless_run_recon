// Retargets calls to Unity 6 renamed members onto their Unity 2022 names (same signatures).
//   PatchApi.exe <in.dll> <out.dll> <searchDir>...
using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class PatchApi
{
    // "<declaring type>::<unity 6 name>" -> unity 2022 name
    static readonly Dictionary<string, string> Renames = new Dictionary<string, string>
    {
        { "UnityEngine.Rigidbody::get_linearVelocity", "get_velocity" },
        { "UnityEngine.Rigidbody::set_linearVelocity", "set_velocity" },
        { "UnityEngine.Rigidbody2D::get_linearVelocity", "get_velocity" },
        { "UnityEngine.Rigidbody2D::set_linearVelocity", "set_velocity" },
    };

    // "<declaring type>::<unity 6 accessor>" -> field that holds the value in the 2022 package
    static readonly Dictionary<string, string> AccessorToField = new Dictionary<string, string>
    {
        { "TMPro.TMP_Asset::get_material", "material" },
        { "TMPro.TMP_Asset::set_material", "material" },
    };

    static int Main(string[] args)
    {
        var resolver = new DefaultAssemblyResolver();
        for (int i = 2; i < args.Length; i++) resolver.AddSearchDirectory(args[i]);
        var module = ModuleDefinition.ReadModule(args[0], new ReaderParameters { AssemblyResolver = resolver });
        int patched = 0;
        foreach (var type in module.GetTypes())
            foreach (var method in type.Methods)
            {
                if (!method.HasBody) continue;
                foreach (Instruction ins in method.Body.Instructions)
                {
                    var target = ins.Operand as MethodReference;
                    if (target == null) continue;
                    string field;
                    if (AccessorToField.TryGetValue(target.DeclaringType.FullName + "::" + target.Name, out field) && target.Resolve() == null)
                    {
                        var definition = target.DeclaringType.Resolve().Fields.Single(f => f.Name == field);
                        ins.OpCode = target.Name.StartsWith("get_") ? OpCodes.Ldfld : OpCodes.Stfld;
                        ins.Operand = module.ImportReference(definition);
                        Console.WriteLine("{0}: {1} -> field {2}", method.FullName, target.Name, field);
                        patched++;
                        continue;
                    }
                    string name;
                    if (!Renames.TryGetValue(target.DeclaringType.FullName + "::" + target.Name, out name)) continue;
                    var replacement = new MethodReference(name, target.ReturnType, target.DeclaringType)
                    {
                        HasThis = target.HasThis, ExplicitThis = target.ExplicitThis, CallingConvention = target.CallingConvention
                    };
                    foreach (var p in target.Parameters) replacement.Parameters.Add(new ParameterDefinition(p.ParameterType));
                    if (replacement.Resolve() == null) throw new Exception("Missing in target: " + replacement.FullName);
                    ins.Operand = module.ImportReference(replacement);
                    Console.WriteLine("{0}: {1} -> {2}", method.FullName, target.Name, name);
                    patched++;
                }
            }
        module.Write(args[1]);
        Console.WriteLine("patched {0} call sites", patched);
        return 0;
    }
}
