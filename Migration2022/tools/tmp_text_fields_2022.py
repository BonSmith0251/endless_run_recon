"""Add TMP 3.0.7 (Unity 2022.3) text fields for values Unity 6's TMP stores under new names.

Unity 6 TMP              ->  TMP 3.0.7
m_TextWrappingMode: 0/3  ->  m_enableWordWrapping: 0   (NoWrap, PreserveWhitespaceNoWrap)
m_TextWrappingMode: 1/2  ->  m_enableWordWrapping: 1   (Normal, PreserveWhitespace)
m_ActiveFontFeatures has 'kern' (6e72656b) -> m_enableKerning: 1, otherwise 0 (overwrites the ignored legacy value)

The Unity 6 fields are kept. Without this, TMP 3.0.7 falls back to its defaults (no wrapping,
no kerning) for every text. Files are backed up before writing.

python -I tmp_text_fields_2022.py --project <project> --backup <dir> --report <json>
"""
import argparse
import json
import re
import shutil
from pathlib import Path

WRAP = re.compile(r"^  m_TextWrappingMode: (\d+)\n", re.M)
FEATURES = re.compile(r"^  m_ActiveFontFeatures:[ ]?(\S*)\n", re.M)
KERNING = re.compile(r"^  m_enableKerning: \d+\n", re.M)


def convert_doc(doc):
    added = 0
    if "m_enableWordWrapping:" not in doc:
        match = WRAP.search(doc)
        if match:
            wrap = 1 if match.group(1) in ("1", "2") else 0
            doc = doc[:match.end()] + f"  m_enableWordWrapping: {wrap}\n" + doc[match.end():]
            added += 1
    match = FEATURES.search(doc)
    if match:
        # Unity 6 also keeps a legacy m_enableKerning (0) that it ignores; set it from the features.
        line = f"  m_enableKerning: {1 if '6e72656b' in match.group(1) else 0}\n"
        old = KERNING.search(doc)
        if old is None:
            doc = doc[:match.end()] + line + doc[match.end():]
            added += 1
        elif old.group(0) != line:
            doc = doc[:old.start()] + line + doc[old.end():]
            added += 1
    return doc, added


def main():
    parser = argparse.ArgumentParser()
    for name in ("project", "backup", "report"):
        parser.add_argument("--" + name, required=True, type=Path)
    args = parser.parse_args()
    report = []
    for path in sorted((args.project / "Assets").rglob("*")):
        if path.suffix not in (".unity", ".prefab", ".asset") or not path.is_file():
            continue
        text = path.read_text(encoding="utf-8", errors="ignore")
        if "m_TextWrappingMode:" not in text and "m_ActiveFontFeatures:" not in text:
            continue
        docs = re.split(r"(?=^--- !u!)", text, flags=re.M)
        total = 0
        for i, doc in enumerate(docs):
            docs[i], added = convert_doc(doc)
            total += added
        if not total:
            continue
        backup = args.backup / path.relative_to(args.project)
        backup.parent.mkdir(parents=True, exist_ok=True)
        if not backup.exists():
            shutil.copy2(path, backup)
        path.write_text("".join(docs), encoding="utf-8", newline="")
        report.append({"file": path.relative_to(args.project).as_posix(), "fields_added": total})
    args.report.write_text(json.dumps(report, indent=1), encoding="utf-8")
    print(json.dumps({"files": len(report), "fields_added": sum(r["fields_added"] for r in report)}))


if __name__ == "__main__":
    main()
