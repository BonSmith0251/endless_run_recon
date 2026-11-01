"""Rewrite Unity 6 PluginImporter metas (serializedVersion 3, platform dictionary) into the
Unity 2022.3 schema (serializedVersion 2, list of first/second pairs).

Keeps the GUID, flags and per-platform settings. Windows-only native DLLs (under an x86_64
folder) are no longer marked for Linux/macOS players. Originals are copied to --backup.

python -I convert_plugin_metas.py --project <project> --backup <dir> [--deps <pyyaml dir>]
"""
import argparse
import json
import shutil
import sys
from pathlib import Path

GROUPS = {
    "Any": ("Any", ""),
    "Editor": ("Editor", "Editor"),
    "Win": ("Standalone", "Win"),
    "Win64": ("Standalone", "Win64"),
    "Linux64": ("Standalone", "Linux64"),
    "OSXUniversal": ("Standalone", "OSXUniversal"),
    "Android": ("Android", "Android"),
    "iOS": ("iPhone", "iOS"),
    "tvOS": ("tvOS", "tvOS"),
    "WebGL": ("WebGL", "WebGL"),
    "WindowsStoreApps": ("Windows Store Apps", "WindowsStoreApps"),
}
ORDER = ["Any", "Editor", "Linux64", "OSXUniversal", "Win", "Win64"]
WINDOWS_ONLY_NATIVE = {"fmodstudio.dll", "resonanceaudio.dll", "steam_api64.dll"}


def scalar(value):
    if value is True:
        return "true"
    if value is False:
        return "false"
    return "" if value is None else str(value)


def convert(meta, text, yaml):
    header, _, rest = text.partition("PluginImporter:\n")
    data = yaml.safe_load("PluginImporter:\n" + rest)["PluginImporter"]
    if data.get("serializedVersion") != 3:
        return None
    platforms = {k: v for k, v in (data.get("platformData") or {}).items() if k in GROUPS}
    unknown = sorted(k for k in (data.get("platformData") or {}) if k not in GROUPS)
    if meta.name[:-5].lower() in WINDOWS_ONLY_NATIVE:
        for name in ("Linux64", "OSXUniversal"):
            if name in platforms:
                platforms[name] = {"enabled": 0, "settings": {"CPU": "None"}}
    out = [header + "PluginImporter:", "  externalObjects: {}", "  serializedVersion: 2"]
    for key in ("iconMap", "executionOrder"):
        out.append(f"  {key}: {{}}")
    constraints = data.get("defineConstraints") or []
    out.append("  defineConstraints: []" if not constraints else "  defineConstraints:")
    out += [f"  - {c}" for c in constraints]
    for key in ("isPreloaded", "isOverridable", "isExplicitlyReferenced", "validateReferences"):
        out.append(f"  {key}: {int(data.get(key) or 0)}")
    out.append("  platformData:")
    for name in sorted(platforms, key=lambda n: (ORDER.index(n) if n in ORDER else 99, n)):
        group, platform = GROUPS[name]
        entry = platforms[name] or {}
        settings = entry.get("settings") or {}
        out += ["  - first:", f"      {group}: {platform}", "    second:", f"      enabled: {int(entry.get('enabled') or 0)}"]
        if settings:
            out.append("      settings:")
            out += [f"        {k}: {scalar(v)}" for k, v in sorted(settings.items())]
        else:
            out.append("      settings: {}")
    for key in ("userData", "assetBundleName", "assetBundleVariant"):
        out.append(f"  {key}: {scalar(data.get(key))}")
    return "\n".join(out) + "\n", unknown


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--project", required=True, type=Path)
    parser.add_argument("--backup", required=True, type=Path)
    parser.add_argument("--deps", type=Path)
    args = parser.parse_args()
    if args.deps:
        sys.path.insert(0, str(args.deps))
    import yaml

    report = []
    for meta in sorted((args.project / "Assets").rglob("*.meta")):
        text = meta.read_text(encoding="utf-8")
        if "PluginImporter:" not in text:
            continue
        result = convert(meta, text, yaml)
        rel = meta.relative_to(args.project).as_posix()
        if result is None:
            report.append({"meta": rel, "action": "unchanged"})
            continue
        backup = args.backup / rel
        backup.parent.mkdir(parents=True, exist_ok=True)
        if not backup.exists():
            shutil.copy2(meta, backup)
        meta.write_text(result[0], encoding="utf-8", newline="\n")
        report.append({"meta": rel, "action": "converted", "dropped_platform_keys": result[1]})
    print(json.dumps(report, indent=1))


if __name__ == "__main__":
    main()
