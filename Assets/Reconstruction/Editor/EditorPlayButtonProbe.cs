using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

// Regression probe: open the gameplay scene in Edit Mode, then enter Play Mode
// exactly through the editor API. Do not load settings or run the audit first.
[InitializeOnLoad]
public static class EditorPlayButtonProbe
{
    const string Key = "EditorPlayButtonProbe.";
    static string Output => Path.GetFullPath(Path.Combine(Application.dataPath, "../../play-mode-repair/evidence"));
    static EditorPlayButtonProbe()
    {
        Application.logMessageReceived += Log;
        EditorApplication.update += Update;
    }
    static void Log(string message, string stack, LogType type)
    {
        if (!SessionState.GetBool(Key + "active", false) ||
            (type != LogType.Error && type != LogType.Exception && type != LogType.Assert)) return;
        if (message.StartsWith("Couldn't resolve achievement with id ") ||
            message.StartsWith("Couldn't resolve stat with id ") ||
            message.StartsWith("Couldn't set stat with id "))
        {
            SessionState.SetInt(Key + "services", SessionState.GetInt(Key + "services", 0) + 1);
            return;
        }
        SessionState.SetInt(Key + "errors", SessionState.GetInt(Key + "errors", 0) + 1);
        File.AppendAllText(Path.Combine(Output, "editor-play-errors.log"), type + ": " + message + "\n" + stack + "\n");
    }
    public static void Begin()
    {
        Directory.CreateDirectory(Output);
        File.WriteAllText(Path.Combine(Output, "editor-play-errors.log"), "");
        SessionState.SetBool(Key + "active", true);
        SessionState.SetInt(Key + "errors", 0);
        SessionState.SetInt(Key + "services", 0);
        SessionState.SetInt(Key + "cycle", 1);
        SessionState.SetInt(Key + "stage", 0);
        SessionState.SetString(Key + "deadline", DateTime.UtcNow.AddSeconds(180).ToString("O"));
        var gameViewType = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
        if (gameViewType != null) { var view = EditorWindow.GetWindow(gameViewType); view.Show(); view.Focus(); }
        EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity", OpenSceneMode.Single);
        SetStage(0);
    }
    static void SetStage(int stage)
    {
        SessionState.SetInt(Key + "stage", stage);
        SessionState.SetString(Key + "time", DateTime.UtcNow.ToString("O"));
    }
    static void Update()
    {
        if (!SessionState.GetBool(Key + "active", false)) return;
        if (DateTime.UtcNow > DateTime.Parse(SessionState.GetString(Key + "deadline", "")).ToUniversalTime())
        {
            Debug.LogError("Editor Play button probe timed out");
            Finish(); return;
        }
        double elapsed = (DateTime.UtcNow - DateTime.Parse(SessionState.GetString(Key + "time", "")).ToUniversalTime()).TotalSeconds;
        int stage = SessionState.GetInt(Key + "stage", 0);
        if (stage == 0 && elapsed > 4)
        {
            SetStage(1); EditorApplication.EnterPlaymode();
        }
        else if (stage == 1 && EditorApplication.isPlaying)
            SetStage(2);
        else if (stage == 2 && elapsed > 25)
        {
            var settings = LocalizationSettings.GetInstanceDontCreateDefault();
            var locale = settings ? LocalizationSettings.SelectedLocale : null;
            if (!settings || !locale || string.IsNullOrEmpty(locale.Identifier.Code)) Debug.LogError("Play button regression: no active settings/locale");
            if (SceneManager.GetActiveScene().name != "SampleScene" || Camera.allCamerasCount == 0)
                Debug.LogError("Play button regression: gameplay scene/camera did not initialize");
            if (!UnityEngine.Object.FindObjectsByType<Monoblok.MainMenu>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Any())
                Debug.LogError("Play button regression: no active MainMenu");
            int cycle = SessionState.GetInt(Key + "cycle", 1);
            var ui = new System.Text.StringBuilder();
            const System.Reflection.BindingFlags statics = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;
            foreach (var field in typeof(Canvas).GetFields(statics))
                if (field.GetValue(null) is Delegate callbacks)
                    ui.AppendLine("Canvas callback " + field.Name + ": " + string.Join(", ", callbacks.GetInvocationList().Select(d => d.Method.DeclaringType + "." + d.Method.Name)));
            var registry = typeof(UnityEngine.UI.CanvasUpdateRegistry).GetField("s_Instance", statics).GetValue(null);
            ui.AppendLine("Registry exists=" + (registry != null));
            if (registry != null)
                foreach (var field in registry.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
                    if (field.Name.Contains("Queue"))
                    {
                        var queue = field.GetValue(registry);
                        ui.AppendLine(field.Name + "=" + queue.GetType().GetProperty("Count").GetValue(queue));
                    }
            ui.AppendLine("Graphics=" + SystemInfo.graphicsDeviceType + " rendersUIOverlay=" + UnityEngine.Rendering.SupportedRenderingFeatures.active.rendersUIOverlay);
            foreach (var canvas in Resources.FindObjectsOfTypeAll<Canvas>().Where(c => c.isActiveAndEnabled && c.gameObject.scene.IsValid()))
                ui.AppendLine("Canvas " + canvas.name + " mode=" + canvas.renderMode + " display=" + canvas.targetDisplay + " rect=" + ((RectTransform)canvas.transform).rect);
            foreach (var graphic in Resources.FindObjectsOfTypeAll<UnityEngine.UI.Graphic>().Where(g => g.isActiveAndEnabled && g.gameObject.scene.IsValid()))
            {
                var mesh = graphic.canvasRenderer.GetMesh();
                ui.AppendLine(graphic.name + " vertices=" + (mesh ? mesh.vertexCount : 0) + " culled=" + graphic.canvasRenderer.cull + " alpha=" + graphic.canvasRenderer.GetInheritedAlpha() + " pos=" + graphic.transform.position + " materials=" + graphic.canvasRenderer.materialCount);
            }
            File.WriteAllText(Path.Combine(Output, "editor-play-ui-" + cycle + ".txt"), ui.ToString());
            var menuGraphic = Resources.FindObjectsOfTypeAll<UnityEngine.UI.Graphic>().FirstOrDefault(g => g.isActiveAndEnabled && g.name == "New Game Button");
            if (!menuGraphic || !menuGraphic.canvasRenderer.GetMesh() || menuGraphic.canvasRenderer.GetMesh().vertexCount == 0)
                Debug.LogError("Play button regression: New Game menu has no rendered geometry");
            ScreenCapture.CaptureScreenshot(Path.Combine(Output, "editor-play-cycle-" + cycle + ".png"));
            File.WriteAllText(Path.Combine(Output, "editor-play-cycle-" + cycle + ".json"),
                JsonUtility.ToJson(new Result { cycle = cycle, scene = SceneManager.GetActiveScene().name,
                    locale = locale ? locale.Identifier.Code : null, settings = settings ? settings.name : null,
                    errors = SessionState.GetInt(Key + "errors", 0), paused = EditorApplication.isPaused,
                    cameras = Camera.allCamerasCount, externalServiceErrors = SessionState.GetInt(Key + "services", 0) }, true));
            SetStage(3);
        }
        else if (stage == 3 && elapsed > 2)
        {
            SetStage(4); EditorApplication.ExitPlaymode();
        }
        else if (stage == 4 && !EditorApplication.isPlayingOrWillChangePlaymode && elapsed > 4)
        {
            if (SessionState.GetInt(Key + "cycle", 1) == 1)
            {
                SessionState.SetInt(Key + "cycle", 2); SetStage(0);
            }
            else Finish();
        }
    }
    [Serializable] class Result
    {
        public int cycle, errors, cameras, externalServiceErrors;
        public string scene, locale, settings;
        public bool paused;
    }
    static void Finish()
    {
        int errors = SessionState.GetInt(Key + "errors", 0);
        File.WriteAllText(Path.Combine(Output, "editor-play-result.json"),
            "{\"errors\":" + errors + ",\"cycles\":" + SessionState.GetInt(Key + "cycle", 1) +
            ",\"passed\":" + (errors == 0 ? "true" : "false") + "}");
        SessionState.SetBool(Key + "active", false);
        EditorApplication.Exit(errors == 0 ? 0 : 8);
    }
}
