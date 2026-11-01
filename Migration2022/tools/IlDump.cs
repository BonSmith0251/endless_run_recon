// Dumps types, fields (with custom attributes and constant/initial values) and method IL.
//   IlDump.exe <assembly.dll> [typeNameFilter]
using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

class IlDump
{
    static string Attrs(ICustomAttributeProvider p)
    {
        return string.Join(" ", p.CustomAttributes.Select(a => "[" + a.AttributeType.Name +
            (a.HasConstructorArguments ? "(" + string.Join(", ", a.ConstructorArguments.Select(x => Convert.ToString(x.Value))) + ")" : "") + "]"));
    }

    static void Main(string[] args)
    {
        var module = ModuleDefinition.ReadModule(args[0]);
        foreach (var type in module.GetTypes())
        {
            if (args.Length > 1 && !type.FullName.Contains(args[1])) continue;
            Console.WriteLine("\n=== {0} : {1} {2} {3}", type.FullName, type.BaseType == null ? "" : type.BaseType.FullName, type.Attributes, Attrs(type));
            foreach (var f in type.Fields)
                Console.WriteLine("  field {0} {1} {2} {3}", f.Attributes, f.FieldType.FullName, f.Name, Attrs(f));
            foreach (var p in type.Properties)
                Console.WriteLine("  prop {0} {1}", p.PropertyType.FullName, p.Name);
            foreach (var m in type.Methods)
            {
                Console.WriteLine("  method {0} {1} {2}", m.Attributes, m.FullName, Attrs(m));
                if (!m.HasBody) continue;
                foreach (Instruction ins in m.Body.Instructions)
                {
                    string operand = ins.Operand is Instruction ? "IL_" + ((Instruction)ins.Operand).Offset.ToString("x4")
                        : ins.Operand is Instruction[] ? string.Join(",", ((Instruction[])ins.Operand).Select(x => "IL_" + x.Offset.ToString("x4")))
                        : Convert.ToString(ins.Operand);
                    Console.WriteLine("      IL_{0:x4} {1} {2}", ins.Offset, ins.OpCode, operand);
                }
            }
        }
    }
}
