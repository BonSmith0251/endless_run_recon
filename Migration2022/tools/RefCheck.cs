// Reports type and member references in the given assemblies that do not resolve
// against a target Unity editor's assemblies. Usage:
//   RefCheck.exe <out.json> --dir <searchDir> [--dir ...] <assembly.dll> [...]
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;

class RefCheck
{
    static string Esc(string s) { return s.Replace("\\", "\\\\").Replace("\"", "\\\""); }

    static int Main(string[] args)
    {
        var dirs = new List<string>();
        var targets = new List<string>();
        string output = args[0];
        for (int i = 1; i < args.Length; i++)
        {
            if (args[i] == "--dir") dirs.Add(args[++i]);
            else targets.Add(args[i]);
        }
        var resolver = new DefaultAssemblyResolver();
        foreach (var d in resolver.GetSearchDirectories()) resolver.RemoveSearchDirectory(d);
        foreach (var d in dirs) resolver.AddSearchDirectory(d);

        var sb = new StringBuilder("[\n");
        int total = 0;
        bool first = true;
        foreach (var path in targets)
        {
            var module = ModuleDefinition.ReadModule(path, new ReaderParameters { AssemblyResolver = resolver });
            var missing = new SortedSet<string>();
            foreach (var t in module.GetTypeReferences())
            {
                try { if (t.Resolve() == null) missing.Add("type " + t.Scope.Name + " :: " + t.FullName); }
                catch (Exception e) { missing.Add("type " + t.Scope.Name + " :: " + t.FullName + " (" + e.GetType().Name + ")"); }
            }
            foreach (var m in module.GetMemberReferences())
            {
                var scope = m.DeclaringType.GetElementType().Scope.Name;
                try
                {
                    bool ok = m is MethodReference ? ((MethodReference)m).Resolve() != null
                        : m is FieldReference ? ((FieldReference)m).Resolve() != null : true;
                    if (!ok) missing.Add("member " + scope + " :: " + m.FullName);
                }
                catch (Exception e) { missing.Add("member " + scope + " :: " + m.FullName + " (" + e.GetType().Name + ")"); }
            }
            total += missing.Count;
            Console.WriteLine("{0}: {1} unresolved", Path.GetFileName(path), missing.Count);
            if (!first) sb.Append(",\n");
            first = false;
            sb.Append("  {\"assembly\": \"" + Esc(Path.GetFileName(path)) + "\", \"unresolved\": [");
            sb.Append(string.Join(", ", missing.Select(s => "\"" + Esc(s) + "\"")));
            sb.Append("]}");
        }
        sb.Append("\n]\n");
        File.WriteAllText(output, sb.ToString());
        return total == 0 ? 0 : 3;
    }
}
