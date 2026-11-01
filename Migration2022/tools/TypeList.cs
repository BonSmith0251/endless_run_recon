// Prints "<assembly>\t<namespace>\t<name>\t<baseType>" for every top-level class in the given assemblies.
using System;
using Mono.Cecil;

class TypeList
{
    static void Main(string[] args)
    {
        foreach (var path in args)
        {
            var module = ModuleDefinition.ReadModule(path);
            foreach (var t in module.Types)
            {
                if (!t.IsClass || t.IsNested) continue;
                Console.WriteLine("{0}\t{1}\t{2}\t{3}", module.Assembly.Name.Name, t.Namespace, t.Name,
                    t.BaseType == null ? "" : t.BaseType.FullName);
            }
        }
    }
}
