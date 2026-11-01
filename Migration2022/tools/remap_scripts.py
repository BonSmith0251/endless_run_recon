"""Point MonoScript references at removed Unity 6 package DLLs to the matching scripts in
the 2022 packages: {fileID: <hash>, guid: <dll>, type: 3} -> {fileID: 11500000, guid: <cs>, type: 3}.

python -I remap_scripts.py --project <project> --removed <dir with removed DLL metas>
       --types <TypeList tsv of removed DLLs> --backup <dir> --report <json> [--apply]
"""
import argparse
import json
import re
import shutil
import sys
from collections import defaultdict
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parent))
from unity_fileid import script_file_id  # noqa: E402

REF = re.compile(r"\{fileID: (-?\d+), guid: ([0-9a-f]{32}), type: 3\}")
NAMESPACE = re.compile(r"^\s*namespace\s+([\w.]+)", re.M)


def guid_of(meta):
    match = re.search(r"^guid: ([0-9a-f]{32})", meta.read_text(encoding="utf-8", errors="ignore"), re.M)
    return match.group(1) if match else None


def package_scripts(package_cache):
    index = defaultdict(list)
    for cs in package_cache.rglob("*.cs"):
        meta = cs.with_name(cs.name + ".meta")
        if not meta.exists():
            continue
        text = cs.read_text(encoding="utf-8", errors="ignore")
        if not re.search(r"\b(class|struct)\s+" + re.escape(cs.stem) + r"\b", text):
            continue
        namespaces = NAMESPACE.findall(text)
        index[(namespaces[0] if namespaces else "", cs.stem)].append((guid_of(meta), cs.relative_to(package_cache).as_posix()))
    return index


def main():
    parser = argparse.ArgumentParser()
    for name in ("project", "removed", "types", "backup", "report"):
        parser.add_argument("--" + name, required=True, type=Path)
    parser.add_argument("--apply", action="store_true")
    args = parser.parse_args()

    dll_guid = {}
    for meta in args.removed.glob("*.dll.meta"):
        dll_guid[meta.name[:-9]] = guid_of(meta)
    by_ref = {}
    for line in args.types.read_text(encoding="utf-8").splitlines():
        assembly, namespace, name, base = line.split("\t")
        if assembly in dll_guid:
            by_ref[(dll_guid[assembly], script_file_id(namespace, name))] = (assembly, namespace, name)
    removed = set(dll_guid.values())
    index = package_scripts(args.project / "Library" / "PackageCache")
    # Project-side replacements for types the 2022 packages lack (key: namespace, name).
    for cs in (args.project / "Assets" / "Reconstruction" / "Compat2022").glob("*.cs"):
        namespaces = NAMESPACE.findall(cs.read_text(encoding="utf-8"))
        index[(namespaces[0] if namespaces else "", cs.stem)] = [
            (guid_of(cs.with_name(cs.name + ".meta")), "Assets/" + cs.relative_to(args.project / "Assets").as_posix())]

    mapping, unresolved, changed_files = {}, defaultdict(int), []
    for root in ("Assets", "ProjectSettings"):
        for path in sorted((args.project / root).rglob("*")):
            if not path.is_file() or path.suffix in (".meta", ".dll", ".cs", ".png", ".bundle", ".bank"):
                continue
            with path.open("rb") as stream:
                if not stream.read(5) == b"%YAML":
                    continue
            text = path.read_text(encoding="utf-8")

            def replace(match):
                file_id, guid = int(match.group(1)), match.group(2)
                if guid not in removed:
                    return match.group(0)
                source = by_ref.get((guid, file_id))
                if source is None:
                    unresolved[f"unknown type {guid}:{file_id}"] += 1
                    return match.group(0)
                candidates = index.get((source[1], source[2]), [])
                if not candidates:
                    # URP 14 declares some types (FullScreenPassRendererFeature) in the global namespace.
                    candidates = index.get(("", source[2]), [])
                if len(candidates) != 1:
                    unresolved[f"{source[0]} {source[1]}.{source[2]} candidates={len(candidates)}"] += 1
                    return match.group(0)
                mapping[f"{guid}:{file_id}"] = {"type": f"{source[1]}.{source[2]}", "assembly": source[0],
                                                "script": candidates[0][1], "guid": candidates[0][0]}
                return "{fileID: 11500000, guid: %s, type: 3}" % candidates[0][0]

            new = REF.sub(replace, text)
            if new != text:
                rel = path.relative_to(args.project).as_posix()
                changed_files.append(rel)
                if args.apply:
                    backup = args.backup / rel
                    backup.parent.mkdir(parents=True, exist_ok=True)
                    if not backup.exists():
                        shutil.copy2(path, backup)
                    path.write_text(new, encoding="utf-8", newline="")
    report = {"applied": args.apply, "changed_files": len(changed_files), "mapped_types": mapping,
              "unresolved": dict(sorted(unresolved.items())), "files": changed_files}
    args.report.write_text(json.dumps(report, indent=1), encoding="utf-8")
    print(json.dumps({"changed_files": len(changed_files), "mapped_types": len(mapping),
                      "unresolved": report["unresolved"]}, indent=1))


if __name__ == "__main__":
    main()
