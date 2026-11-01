// Ports VhsFx.Runtime.dll (VolFx, built for URP 17) to URP 14 by keeping its legacy
// ScriptableRenderPass.Execute path and removing the RenderGraph path. Type names, namespaces
// and serialized fields are unchanged, so existing asset references keep working.
//   PatchVhsFx.exe <in.dll> <out.dll> <searchDir>...
using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class PatchVhsFx
{
    static ModuleDefinition module;

    static TypeDefinition Type(string fullName)
    {
        var t = module.GetTypes().FirstOrDefault(x => x.FullName == fullName);
        if (t == null) throw new Exception("type not found: " + fullName);
        return t;
    }

    static AssemblyDefinition Load(DefaultAssemblyResolver resolver, string name)
    {
        return resolver.Resolve(new AssemblyNameReference(name, new Version(0, 0, 0, 0)));
    }

    static int Main(string[] args)
    {
        var resolver = new DefaultAssemblyResolver();
        for (int i = 2; i < args.Length; i++) resolver.AddSearchDirectory(args[i]);
        module = ModuleDefinition.ReadModule(args[0], new ReaderParameters { AssemblyResolver = resolver });
        var urp = Load(resolver, "Unity.RenderPipelines.Universal.Runtime").MainModule;

        // 1. Drop the RenderGraph implementation.
        var execution = Type("VolFx.VhsFx/PassExecution");
        foreach (var m in execution.Methods.Where(m => m.Name == "RecordRenderGraph" || m.Name.Contains("<RecordRenderGraph>")
                     || (m.Name == "Execute" && m.Parameters.Count == 2 && m.Parameters[0].ParameterType.Name == "PassData")).ToList())
            execution.Methods.Remove(m);
        execution.NestedTypes.Remove(execution.NestedTypes.Single(t => t.Name == "PassData"));
        foreach (var f in execution.Fields.Where(f => f.Name == "_initApiRg" || f.Name == "_callApiRg").ToList())
            execution.Fields.Remove(f);
        var init = execution.Methods.Single(m => m.Name == "Init");
        var il = init.Body.GetILProcessor();
        foreach (var ins in init.Body.Instructions.ToList())
        {
            var ctor = ins.Operand as MethodReference;
            if (ins.OpCode == OpCodes.Newobj && ctor != null && (ctor.DeclaringType.Name == "InitApiRg" || ctor.DeclaringType.Name == "CallApiRg"))
            {
                // ldarg.0; newobj X::.ctor; stfld _xRg
                il.Remove(ins.Previous);
                il.Remove(ins.Next);
                il.Remove(ins);
            }
        }
        var volFx = Type("VolFx.VolFx");
        foreach (var name in new[] { "InitApiRg", "CallApiRg" })
            volFx.NestedTypes.Remove(volFx.NestedTypes.Single(t => t.Name == name));

        // 2. URP 17 ref-returning CameraData getters -> URP 14 fields (same stack effect).
        var cameraData = urp.GetType("UnityEngine.Rendering.Universal.CameraData");
        var fieldFor = new[] { "renderer", "cameraTargetDescriptor" }.ToDictionary(n => "get_" + n,
            n => module.ImportReference(cameraData.Fields.Single(f => f.Name == n)));

        // 3. ReAllocateHandleIfNeeded(ref h, ref desc, filter, wrap, aniso, bias, name)
        //    -> helper calling URP 14 ReAllocateIfNeeded(ref h, in desc, filter, wrap, false, aniso, bias, name).
        var rendering = urp.GetType("UnityEngine.Rendering.Universal.RenderingUtils");
        var reallocate = module.ImportReference(rendering.Methods.Single(m => m.Name == "ReAllocateIfNeeded"
            && m.Parameters.Count == 8 && m.Parameters[1].ParameterType.IsByReference));
        var utils = Type("VolFx.Utils");
        MethodDefinition helper = null;

        int retargeted = 0;
        foreach (var type in module.GetTypes())
            foreach (var method in type.Methods.Where(m => m.HasBody))
                foreach (var ins in method.Body.Instructions)
                {
                    var target = ins.Operand as MethodReference;
                    if (target == null) continue;
                    if (target.DeclaringType.FullName == cameraData.FullName && fieldFor.ContainsKey(target.Name))
                    {
                        ins.OpCode = OpCodes.Ldflda;
                        ins.Operand = fieldFor[target.Name];
                        retargeted++;
                    }
                    else if (target.Name == "ReAllocateHandleIfNeeded")
                    {
                        if (helper == null) helper = MakeHelper(utils, target, reallocate);
                        ins.Operand = helper;
                        retargeted++;
                    }
                }

        // 4. IPostProcessComponent moved from Core 17 to URP 14 (UnityEngine.Rendering.Universal),
        //    where it also requires IsTileCompatible().
        var vol = Type("VolFx.VhsVol");
        var oldInterface = vol.Interfaces.Single(i => i.InterfaceType.Name == "IPostProcessComponent");
        vol.Interfaces.Remove(oldInterface);
        var urpInterface = urp.GetType("UnityEngine.Rendering.Universal.IPostProcessComponent");
        vol.Interfaces.Add(new InterfaceImplementation(module.ImportReference(urpInterface)));
        var tile = new MethodDefinition("IsTileCompatible",
            MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
            module.TypeSystem.Boolean);
        tile.Body.GetILProcessor().Emit(OpCodes.Ldc_I4_0);
        tile.Body.GetILProcessor().Emit(OpCodes.Ret);
        vol.Methods.Add(tile);

        module.Write(args[1]);
        Console.WriteLine("retargeted {0} call sites; RenderGraph path removed", retargeted);
        return 0;
    }

    static MethodDefinition MakeHelper(TypeDefinition owner, MethodReference old, MethodReference target)
    {
        var helper = new MethodDefinition("ReAllocateHandleIfNeededCompat",
            MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig, module.TypeSystem.Boolean);
        foreach (var p in old.Parameters) helper.Parameters.Add(new ParameterDefinition(p.Name, ParameterAttributes.None, p.ParameterType));
        var il = helper.Body.GetILProcessor();
        il.Emit(OpCodes.Ldarg_0);   // ref RTHandle
        il.Emit(OpCodes.Ldarg_1);   // ref/in RenderTextureDescriptor
        il.Emit(OpCodes.Ldarg_2);   // FilterMode
        il.Emit(OpCodes.Ldarg_3);   // TextureWrapMode
        il.Emit(OpCodes.Ldc_I4_0);  // isShadowMap: false
        il.Emit(OpCodes.Ldarg_S, helper.Parameters[4]); // anisoLevel
        il.Emit(OpCodes.Ldarg_S, helper.Parameters[5]); // mipMapBias
        il.Emit(OpCodes.Ldarg_S, helper.Parameters[6]); // name
        il.Emit(OpCodes.Call, target);
        il.Emit(OpCodes.Ret);
        owner.Methods.Add(helper);
        return helper;
    }
}
