// Lists methods whose signature or body references a member/type whose full name contains <text>.
//   FindUses.exe <assembly.dll> <text>
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

class FindUses
{
    static void Main(string[] args)
    {
        var module = ModuleDefinition.ReadModule(args[0]);
        foreach (var type in module.GetTypes())
        {
            if (type.BaseType != null && type.BaseType.FullName.Contains(args[1]))
                Console.WriteLine("base  {0}", type.FullName);
            foreach (var field in type.Fields)
                if (field.FieldType.FullName.Contains(args[1])) Console.WriteLine("field {0}", field.FullName);
            foreach (var method in type.Methods)
            {
                bool hit = method.FullName.Contains(args[1]);
                if (method.HasBody)
                    foreach (Instruction ins in method.Body.Instructions)
                    {
                        var member = ins.Operand as MemberReference;
                        if (member != null && member.FullName.Contains(args[1])) { hit = true; break; }
                    }
                if (hit) Console.WriteLine("method {0}", method.FullName);
            }
        }
    }
}
