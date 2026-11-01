using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

// Unity 2022.3 migration: the original Addressables 2.9 bundles and binary catalog cannot be
// loaded by the 2022 runtime. Rebuild them from the recovered Locales and tables through the
// real Localization editor, which registers the same Addressables keys and Locale labels.
//   Unity -batchmode -projectPath <project> -executeMethod Migration2022.RebuildLocalization
public static class Migration2022
{
    const string SettingsPath = "Assets/MonoBehaviour/Localization Settings.asset";
    const string ReportPath = "../migration-2022/evidence/localization-rebuild.json";

    [Serializable] class Collection { public string name, path, type; public int tables; public List<string> locales = new List<string>(); }
    [Serializable] class Report
    {
        public string unityVersion; public int locales; public List<Collection> collections = new List<Collection>();
        public List<string> groups = new List<string>(); public int entries; public List<string> labels = new List<string>();
        public string addressablesBuildError; public List<string> problems = new List<string>(); public bool passed;
    }

    [Serializable] class SinkRow { public string asset; public int conditions; public List<string> types = new List<string>(); public List<string> configs = new List<string>(); }
    [Serializable] class SinkReport { public string unityVersion; public List<SinkRow> sinks = new List<SinkRow>(); public List<string> problems = new List<string>(); public bool passed; }

    // GenericGameEventSinkData stores [SerializeReference] conditions; Unity 2022.3 rejects the
    // original generic element type. Confirm the stored conditions load through the 2022 field.
    public static void VerifyAchievementConditions()
    {
        var report = new SinkReport { unityVersion = Application.unityVersion };
        var unsupported = new List<string>();
        Application.LogCallback capture = (message, stack, type) => { if (message.Contains("generic instance type")) unsupported.Add(message); };
        Application.logMessageReceived += capture;
        try
        {
            foreach (var guid in AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets/MonoBehaviour" }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadMainAssetAtPath(path);
                if (asset == null || asset.GetType().BaseType == null || !asset.GetType().BaseType.IsGenericType ||
                    asset.GetType().BaseType.GetGenericTypeDefinition().Name != "GenericGameEventSinkData`1") continue;
                var field = asset.GetType().BaseType.GetField("conditions", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var values = (field.GetValue(asset) as Array) ?? Array.Empty<object>();
                var row = new SinkRow { asset = path, conditions = values.Length };
                foreach (var condition in values)
                {
                    row.types.Add(condition == null ? "<null>" : condition.GetType().FullName);
                    var config = condition?.GetType().GetField("config", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                    var target = config?.GetValue(condition) as UnityEngine.Object;
                    row.configs.Add(target ? AssetDatabase.GetAssetPath(target) : "<none>");
                    if (condition == null || (config != null && !target)) report.problems.Add(path + ": condition did not load");
                }
                // Compare with the conditions serialized in the asset (an empty list is valid data).
                var yaml = File.ReadAllText(path);
                int stored = System.Text.RegularExpressions.Regex.Matches(
                    System.Text.RegularExpressions.Regex.Match(yaml, @"\n  conditions:(?:\n  - rid: \d+)*").Value, @"- rid:").Count;
                if (values.Length != stored) report.problems.Add(path + ": loaded " + values.Length + " of " + stored + " stored conditions");
                report.sinks.Add(row);
            }
            if (report.sinks.Count == 0) report.problems.Add("no GenericGameEventSinkData assets found");
        }
        catch (Exception e) { report.problems.Add(e.ToString()); }
        finally { Application.logMessageReceived -= capture; }
        report.problems.AddRange(unsupported);
        report.passed = report.problems.Count == 0;
        File.WriteAllText("../migration-2022/evidence/achievement-conditions.json", JsonUtility.ToJson(report, true));
        Debug.Log("Migration2022.VerifyAchievementConditions passed=" + report.passed);
        if (Application.isBatchMode) EditorApplication.Exit(report.passed ? 0 : 1);
    }

    public static void RebuildLocalization()
    {
        var report = new Report { unityVersion = Application.unityVersion };
        try
        {
            // Keep 2022 test saves apart from the Unity 6 reconstruction's isolated test saves.
            PlayerSettings.productName = "EndlessRails-Reconstructed-2022";
            AssetDatabase.SaveAssets();

            var aa = AddressableAssetSettingsDefaultObject.GetSettings(true);
            aa.BuildAddressablesWithPlayerBuild = AddressableAssetSettings.PlayerBuildOption.BuildWithPlayer;

            var settings = AssetDatabase.LoadAssetAtPath<LocalizationSettings>(SettingsPath);
            if (!settings) throw new InvalidOperationException("Missing " + SettingsPath);
            LocalizationEditorSettings.ActiveLocalizationSettings = settings;

            foreach (var guid in AssetDatabase.FindAssets("t:Locale", new[] { "Assets/Localization/Locales" }))
            {
                LocalizationEditorSettings.AddLocale(AssetDatabase.LoadAssetAtPath<Locale>(AssetDatabase.GUIDToAssetPath(guid)));
                report.locales++;
            }

            var tables = AssetDatabase.FindAssets("t:LocalizationTable", new[] { "Assets/Localization/Tables" })
                .Select(g => AssetDatabase.LoadAssetAtPath<LocalizationTable>(AssetDatabase.GUIDToAssetPath(g)))
                .Where(t => t).ToList();
            foreach (var group in tables.GroupBy(t => t.SharedData))
            {
                if (!group.Key) { report.problems.Add("tables without SharedTableData: " + string.Join(", ", group.Select(t => t.name))); continue; }
                var collection = LocalizationEditorSettings.GetCollectionForSharedTableData(group.Key);
                if (!collection)
                {
                    string dir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(group.Key)).Replace('\\', '/');
                    collection = LocalizationEditorSettings.CreateCollectionFromLooseTables(group.ToList<LocalizationTable>(),
                        dir + "/" + group.Key.TableCollectionName + ".asset");
                }
                var row = new Collection { name = collection.TableCollectionName, path = AssetDatabase.GetAssetPath(collection),
                    type = collection.GetType().Name, tables = collection.Tables.Count };
                row.locales.AddRange(collection.Tables.Select(t => t.asset ? t.asset.LocaleIdentifier.Code : "<missing>"));
                if (row.tables != group.Count()) report.problems.Add(row.name + ": " + row.tables + " of " + group.Count() + " tables in collection");
                report.collections.Add(row);
            }
            AssetDatabase.SaveAssets();

            foreach (var group in aa.groups.Where(g => g))
            {
                report.groups.Add(group.Name + " (" + group.entries.Count + ")");
                report.entries += group.entries.Count;
                foreach (var entry in group.entries) report.labels.AddRange(entry.labels);
            }
            report.labels = report.labels.Distinct().OrderBy(l => l).ToList();
            foreach (var t in tables)
                if (aa.FindAssetEntry(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(t))) == null)
                    report.problems.Add("table not addressable: " + t.name);

            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
            report.addressablesBuildError = result.Error;
            if (!string.IsNullOrEmpty(result.Error)) report.problems.Add("Addressables build: " + result.Error);
            report.passed = report.problems.Count == 0;
        }
        catch (Exception e)
        {
            report.problems.Add(e.ToString());
        }
        Directory.CreateDirectory(Path.GetDirectoryName(ReportPath));
        File.WriteAllText(ReportPath, JsonUtility.ToJson(report, true));
        Debug.Log("Migration2022.RebuildLocalization passed=" + report.passed + " problems=" + report.problems.Count);
        if (Application.isBatchMode) EditorApplication.Exit(report.passed ? 0 : 1);
    }
}
