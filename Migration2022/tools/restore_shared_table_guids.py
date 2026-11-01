"""Give each Localization SharedTableData asset its original project GUID again.

The game addresses string/asset tables by the SharedTableData's TableCollectionNameGuid, which
the Localization package keeps equal to that asset's own GUID. The export assigned new GUIDs,
so Addressables cannot find them. For each SharedTableData whose meta GUID differs from its
m_TableCollectionNameGuidString, rewrite the meta and every serialized reference to it
("guid: <old>" and Addressables "m_GUID: <old>"). Dry run unless --apply.

python -I restore_shared_table_guids.py --project <project> --backup <dir> --report <json> [--apply]
"""
import argparse
import json
import re
import shutil
from pathlib import Path

SKIP = {".png", ".bundle", ".bank", ".dll", ".ttf", ".otf", ".wav", ".ogg", ".mp4", ".exr", ".cs", ".hlsl", ".shader"}


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--project", required=True, type=Path)
    parser.add_argument("--backup", required=True, type=Path)
    parser.add_argument("--report", required=True, type=Path)
    parser.add_argument("--apply", action="store_true")
    args = parser.parse_args()

    metas = {}
    for meta in (args.project / "Assets").rglob("*.meta"):
        match = re.search(r"^guid: ([0-9a-f]{32})", meta.read_text(encoding="utf-8", errors="ignore"), re.M)
        if match:
            metas[match.group(1)] = meta

    renames = {}
    for asset in (args.project / "Assets" / "Localization").rglob("*Shared Data.asset"):
        text = asset.read_text(encoding="utf-8")
        original = re.search(r"m_TableCollectionNameGuidString: ([0-9a-f]{32})", text)
        meta = asset.with_name(asset.name + ".meta")
        current = re.search(r"^guid: ([0-9a-f]{32})", meta.read_text(encoding="utf-8"), re.M).group(1)
        if not original or original.group(1) == current:
            continue
        if original.group(1) in metas:
            raise SystemExit(f"original GUID {original.group(1)} already used by {metas[original.group(1)]}")
        renames[current] = {"to": original.group(1), "asset": asset.relative_to(args.project).as_posix()}

    pattern = re.compile(r"(guid: |m_GUID: )(" + "|".join(renames) + r")\b") if renames else None
    changed = []
    for root in ("Assets", "ProjectSettings"):
        for path in (args.project / root).rglob("*"):
            if not path.is_file() or path.suffix.lower() in SKIP:
                continue
            try:
                text = path.read_text(encoding="utf-8") if path.stat().st_size < 64 << 20 else None
            except UnicodeDecodeError:
                text = None  # binary payload, cannot hold a text reference
            if text is None or not pattern.search(text):
                continue
            new = pattern.sub(lambda m: m.group(1) + renames[m.group(2)]["to"], text)
            changed.append({"file": path.relative_to(args.project).as_posix(), "replacements": len(pattern.findall(text))})
            if args.apply:
                backup = args.backup / path.relative_to(args.project)
                backup.parent.mkdir(parents=True, exist_ok=True)
                if not backup.exists():
                    shutil.copy2(path, backup)
                path.write_text(new, encoding="utf-8", newline="")
    report = {"applied": args.apply, "renames": renames, "changed_files": changed}
    args.report.write_text(json.dumps(report, indent=1), encoding="utf-8")
    print(json.dumps({"assets": len(renames), "files": len(changed), "replacements": sum(c["replacements"] for c in changed)}))


if __name__ == "__main__":
    main()
