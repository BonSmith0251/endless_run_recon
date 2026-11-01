"""Restore scene transform fields that drifted from the original build, using the mismatches
reported by compare_scene_graphs_2022.py. Only the listed field line inside the listed object
document is rewritten; everything else in the scene is left byte-for-byte unchanged.

python -I restore_scene_fields.py --scene <SampleScene.unity> --comparison <json> --backup <dir>
"""
import argparse
import json
import re
import shutil
import struct
from pathlib import Path


def f32(value):
    """Shortest text that round-trips as the same float32, the way Unity writes floats."""
    target = struct.pack("<f", value)
    for digits in range(1, 10):
        text = "%.*g" % (digits, value)
        if struct.pack("<f", float(text)) == target:
            if "e" in text:  # %g switches to exponent form for e.g. 40 at 1 digit
                text = repr(float(text))
                text = text[:-2] if text.endswith(".0") else text
            return text
    return repr(value)


def vector(value):
    return "{" + ", ".join(f"{k}: {f32(v)}" for k, v in value.items()) + "}"


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--scene", required=True, type=Path)
    parser.add_argument("--comparison", required=True, type=Path)
    parser.add_argument("--backup", required=True, type=Path)
    args = parser.parse_args()

    mismatches = [m for s in json.loads(args.comparison.read_text(encoding="utf-8"))
                  if s["scene"] == args.scene.stem for m in s["mismatches"]]
    text = args.scene.read_text(encoding="utf-8")
    starts = [(m.start(), m.group(1)) for m in re.finditer(r"^--- !u!\d+ &(-?\d+)", text, re.M)]
    bounds = {pid: (start, starts[i + 1][0] if i + 1 < len(starts) else len(text)) for i, (start, pid) in enumerate(starts)}

    edits = []
    for m in mismatches:
        start, end = bounds[m["path_id"]]
        doc = text[start:end]
        line = re.search(r"^(  %s: )(\{[^}\n]*\}|[^\n]*)$" % re.escape(m["field"]), doc, re.M)
        if not line:
            raise SystemExit(f"field {m['field']} not found in &{m['path_id']}")
        value = vector(m["source"]) if isinstance(m["source"], dict) else f32(m["source"])
        edits.append((start + line.start(2), start + line.end(2), value, m))
    for begin, finish, value, _ in sorted(edits, reverse=True):
        text = text[:begin] + value + text[finish:]

    args.backup.mkdir(parents=True, exist_ok=True)
    backup = args.backup / args.scene.name
    if not backup.exists():
        shutil.copy2(args.scene, backup)
    args.scene.write_text(text, encoding="utf-8", newline="")
    print(json.dumps({"restored_fields": len(edits), "objects": len({e[3]["path_id"] for e in edits})}))


if __name__ == "__main__":
    main()
