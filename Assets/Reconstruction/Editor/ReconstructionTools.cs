using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class ReconstructionTools
{
    static Report ActiveAuditReport;
    static readonly HashSet<string> SmokeErrors = new HashSet<string>();
    static ReconstructionTools()
    {
        EditorApplication.update += SmokeUpdate;
        Application.logMessageReceived += RecordSmokeLog;
    }
    static void RecordSmokeLog(string message, string stack, LogType type)
    {
        if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert) return;
        if (ActiveAuditReport != null) ActiveAuditReport.loggedErrors.Add(message+"\n"+stack);
        if (!SessionState.GetBool("ReconstructionSmoke", false)) return;
        string category = "runtime";
        if (stack.Contains("UnityEditor.Search.SearchDatabase") && stack.Contains("UnityEditor.Search.SearchInit")) category = "editor-search";
        else if (message.StartsWith("Couldn't resolve achievement with id ") && stack.Contains("Monoblok.AchievementData:GetAchievement")) category = "external-steam-achievements";
        else if ((message.StartsWith("Couldn't resolve stat with id ") && stack.Contains("Monoblok.StatData:GetStat")) || (message.StartsWith("Couldn't set stat with id ") && stack.Contains("Monoblok.StatData:Trigger"))) category = "external-steam-stats";
        if (!SmokeErrors.Add(category + ":" + message)) return;
        Directory.CreateDirectory(Evidence);
        SessionState.SetInt("ReconstructionErrors-" + category, SessionState.GetInt("ReconstructionErrors-" + category, 0) + 1);
        File.AppendAllText(Path.Combine(Evidence, "smoke-errors.log"), category + " | " + type + ": " + message + "\n" + stack + "\n");
    }
    [Serializable] public class ObjectRow
    {
        public string scene, path, name, sourceId;
        public int sibling, layer;
        public bool active;
        public Vector3 localPosition, localScale;
        public Quaternion localRotation;
        public string[] components;
        public int missingScripts;
    }
    [Serializable] public class ScriptRow { public string assetPath, guid, localId, type, assembly; }
    [Serializable] public class AssetIssue { public string assetPath, error; }
    [Serializable] public class SmokeReport
    {
        public string unityVersion, scene, persistentDataPath, screenshot, captureMethod, verdict, scope;
        public int sceneObjects, cameras, runtimeErrors, editorSearchErrors, externalSteamErrors, externalSteamStatErrors, screenshotWidth, screenshotHeight, activeCanvases, activeGraphics;
        public bool graphicsDeviceAvailable, screenshotExists;
        public List<string> failures = new List<string>();
    }
    [Serializable] public class PlayerBuildReport { public string result, output, scope, exception; public int errors, warnings, terrainVariantsStripped, finalPostVariantsStripped, finalPostVariantsKept, finalPostXRVariantsStripped, uberPostVariantsStripped, uberPostVariantsKept, uberPostXRVariantsStripped; public bool graphicsSettingsRestored; }
    [Serializable] public class RuntimeUiReport { public List<string> canvases = new List<string>(); public List<string> graphics = new List<string>(); }
    [Serializable] public class ReferenceIssue { public string owner, property; public string entityId; }

    [Serializable] public class Report
    {
        public string unityVersion;
        public List<ObjectRow> objects = new List<ObjectRow>();
        public List<ScriptRow> scripts = new List<ScriptRow>();
        public List<AssetIssue> issues = new List<AssetIssue>();
        public int missingSceneScripts, missingPrefabScripts, prefabs, scriptableAssets, objectReferencesChecked, serializedAssetFiles, serializedSubAssets;
        public string scope = "Both build scenes, all prefabs and .asset/.playable/.controller/.overrideController/.anim/.mat/.renderTexture main and sub-assets; unresolved nonzero serialized object references. Original-null references and fidelity are separate checks.";
        public List<ReferenceIssue> missingObjectReferences = new List<ReferenceIssue>();
        public List<string> loggedErrors = new List<string>();
    }
    static string Evidence => Path.GetFullPath(Path.Combine(Application.dataPath, "../../migration-2022/evidence/harness"));

    public static void ConfigureAndAudit()
    {
        foreach(var path in new[]{"Assets/Plugins/x86_64/fmodstudio.dll","Assets/Plugins/x86_64/resonanceaudio.dll","Assets/Plugins/x86_64/steam_api64.dll"})
        {
            var importer=AssetImporter.GetAtPath(path) as PluginImporter;
            importer.SetCompatibleWithAnyPlatform(false);
            importer.SetCompatibleWithEditor(true);
            importer.SetEditorData("CPU","x86_64");
            importer.SetEditorData("OS","Windows");
            importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64,true);
            importer.SetPlatformData(BuildTarget.StandaloneWindows64,"CPU","x86_64");
            importer.SaveAndReimport();
        }
        PlayerSettings.SetPreloadedAssets(new[] {
            AssetDatabase.LoadMainAssetAtPath("Assets/MonoBehaviour/Input Actions.asset"),
            AssetDatabase.LoadMainAssetAtPath("Assets/MonoBehaviour/InputSystem.inputsettings.asset"),
            AssetDatabase.LoadMainAssetAtPath("Assets/MonoBehaviour/Localization Settings.asset")
        });
        var actions=AssetDatabase.LoadAssetAtPath<UnityEngine.InputSystem.InputActionAsset>("Assets/MonoBehaviour/Input Actions.asset");
        UnityEngine.InputSystem.InputSystem.actions=actions;
        EditorBuildSettings.AddConfigObject("com.unity.input.settings.actions",actions,true);
        var globalSettings=AssetDatabase.LoadAssetAtPath<UnityEngine.Rendering.RenderPipelineGlobalSettings>("Assets/MonoBehaviour/UniversalRenderPipelineGlobalSettings.asset");
        var register=typeof(UnityEngine.Rendering.GraphicsSettings).GetMethod("RegisterRenderPipelineSettings",new[]{typeof(Type),typeof(UnityEngine.Rendering.RenderPipelineGlobalSettings)});
        register.Invoke(null,new object[]{typeof(UnityEngine.Rendering.Universal.UniversalRenderPipeline),globalSettings});
        UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline=AssetDatabase.LoadAssetAtPath<UnityEngine.Rendering.RenderPipelineAsset>("Assets/MonoBehaviour/UniversalRP.asset");
        EditorUtility.SetDirty(Unsupported.GetSerializedAssetInterfaceSingleton("GraphicsSettings"));
        PlayerSettings.colorSpace=ColorSpace.Linear;
        PlayerSettings.defaultScreenWidth=1920;
        PlayerSettings.defaultScreenHeight=1080;
        PlayerSettings.companyName="UnityReconstruction";
        PlayerSettings.productName="EndlessRails-Reconstructed-2022";
        var settings=new SerializedObject(Unsupported.GetSerializedAssetInterfaceSingleton("PlayerSettings"));
        var input=settings.FindProperty("activeInputHandler");
        if(input!=null) { input.intValue=1; settings.ApplyModifiedPropertiesWithoutUndo(); }
        AssetDatabase.SaveAssets();
        Debug.Log("RECONSTRUCTION_SETTINGS_RESTORED preloadedAssets=3 linearColor=true input=new");
        Audit();
    }

    public static void ConstructorProbe()
    {
        var go=new GameObject("ReconstructionConstructorProbe");
        go.SetActive(false);
        Debug.Log("CONSTRUCTOR_PROBE_BEGIN");
        go.AddComponent<Monoblok.Upgrades.AddCartActionView>();
        Debug.Log("CONSTRUCTOR_PROBE_CREATED");
        UnityEngine.Object.DestroyImmediate(go);
        Audit();
    }

    public static void Smoke()
    {
        Directory.CreateDirectory(Evidence);
        SmokeErrors.Clear();
        InteractionStage=0; Interaction=null;
        SessionState.SetBool("ReconstructionInteraction",false);
        SessionState.SetBool("ReconstructionGameplay",false);
        SessionState.SetBool("ReconstructionForceIntro",false);
        SessionState.SetBool("ReconstructionFirstTimeSaved",false);
        Gameplay=null;
        SessionState.SetBool("ReconstructionGameplayCaptureRequested",false);
        SessionState.SetBool("ReconstructionShutdownPending",false);
        SessionState.EraseString("ReconstructionEnteredEditModeSince");
        foreach (var category in new[] { "runtime", "editor-search", "external-steam-achievements", "external-steam-stats" }) SessionState.SetInt("ReconstructionErrors-" + category, 0);
        foreach (var file in new[] { "reconstructed-game.png", "smoke-report.json", "smoke-timeout.txt", "render-capture-error.txt", "interaction-report.json", "interaction-selector.png", "interaction-playing.png", "smoke-shutdown-report.json", "gameplay-report.json", "gameplay-wave1.png" })
        {
            string path = Path.Combine(Evidence, file);
            if (File.Exists(path)) File.Delete(path);
        }
        PlayerSettings.companyName="UnityReconstruction";
        PlayerSettings.productName="EndlessRails-Reconstructed-2022";
        SessionState.SetBool("ReconstructionSmoke",true);
        SessionState.SetBool("ReconstructionCaptureRequested",false);
        SessionState.SetString("ReconstructionStarted",DateTime.UtcNow.ToString("O"));
        SessionState.EraseString("ReconstructionPlayingSince");
        File.WriteAllText(Path.Combine(Evidence,"smoke-errors.log"),"");
        // ScreenCapture records the actual Game View including overlay UI. Do not reconfigure
        // runtime Canvases or overwrite this evidence with a single-camera approximation.
        var gameViewType = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
        if (gameViewType != null) { var view = EditorWindow.GetWindow(gameViewType); view.Show(); view.Focus(); }
        EditorSceneManager.OpenScene("Assets/Scenes/Bootstrap.unity",OpenSceneMode.Single);
        EditorApplication.EnterPlaymode();
    }

    static void SmokeUpdate()
    {
        if(!SessionState.GetBool("ReconstructionSmoke",false)) return;
        if(SessionState.GetBool("ReconstructionShutdownPending",false)) { CompleteSmokeShutdown(); return; }
        var started=DateTime.Parse(SessionState.GetString("ReconstructionStarted",DateTime.UtcNow.ToString("O"))).ToUniversalTime();
        if((DateTime.UtcNow-started).TotalSeconds>150)
        {
            File.WriteAllText(Path.Combine(Evidence,"smoke-timeout.txt"),"Timed out entering or testing play mode.");
            FinishSmoke(true); return;
        }
        if(!EditorApplication.isPlaying) return;
        string playing=SessionState.GetString("ReconstructionPlayingSince","");
        if(playing=="") { SessionState.SetString("ReconstructionPlayingSince",DateTime.UtcNow.ToString("O")); return; }
        var elapsed=(DateTime.UtcNow-DateTime.Parse(playing).ToUniversalTime()).TotalSeconds;
        if(elapsed<25) return;
        if(!SessionState.GetBool("ReconstructionCaptureRequested",false))
        {
            SessionState.SetBool("ReconstructionCaptureRequested",true);
            try
            {
                var ui = new RuntimeUiReport();
                foreach (var canvas in Resources.FindObjectsOfTypeAll<Canvas>().Where(c=>c.gameObject.scene.IsValid()))
                    ui.canvases.Add(GetPath(canvas.transform)+" active="+canvas.isActiveAndEnabled+" mode="+canvas.renderMode+" camera="+(canvas.worldCamera ? canvas.worldCamera.name : "null")+" scale="+canvas.scaleFactor);
                foreach (var graphic in Resources.FindObjectsOfTypeAll<UnityEngine.UI.Graphic>().Where(g=>g.isActiveAndEnabled && g.gameObject.scene.IsValid()))
                    ui.graphics.Add(GetPath(graphic.transform)+" type="+graphic.GetType().Name+" rect="+graphic.rectTransform.rect+" material="+(graphic.material ? graphic.material.name : "null")+" shader="+(graphic.material && graphic.material.shader ? graphic.material.shader.name : "null")+" alpha="+graphic.color.a+" culled="+graphic.canvasRenderer.cull);
                File.WriteAllText(Path.Combine(Evidence,"runtime-ui.json"),JsonUtility.ToJson(ui,true));
                ScreenCapture.CaptureScreenshot(Path.Combine(Evidence,"reconstructed-game.png"));
                Debug.Log("RECONSTRUCTION_SMOKE_CAPTURE_REQUEST scene="+SceneManager.GetActiveScene().name);
            }
            catch(Exception error) { File.WriteAllText(Path.Combine(Evidence,"render-capture-error.txt"),error.ToString()); Debug.LogException(error); }
        }
        if(elapsed>35)
        {
            if(SessionState.GetBool("ReconstructionInteraction",false)) InteractionUpdate(elapsed);
            else FinishSmoke(false);
        }
    }

    static void FinishSmoke(bool timedOut, bool exitEditor=true)
    {
        var report=new SmokeReport {
            unityVersion=Application.unityVersion, scene=SceneManager.GetActiveScene().name,
            persistentDataPath=Application.persistentDataPath, screenshot=Path.Combine(Evidence,"reconstructed-game.png"),
            captureMethod="ScreenCapture.CaptureScreenshot; actual Game View, no Canvas/camera changes",
            scope="35-second Bootstrap to SampleScene startup, active camera/UI and runtime exceptions; does not verify gameplay, visual fidelity or unavailable Steam services",
            sceneObjects=Resources.FindObjectsOfTypeAll<GameObject>().Count(g=>g.scene.IsValid() && g.scene.isLoaded),
            cameras=Camera.allCamerasCount, graphicsDeviceAvailable=SystemInfo.graphicsDeviceType!=UnityEngine.Rendering.GraphicsDeviceType.Null,
            runtimeErrors=SessionState.GetInt("ReconstructionErrors-runtime",0), editorSearchErrors=SessionState.GetInt("ReconstructionErrors-editor-search",0),
            externalSteamErrors=SessionState.GetInt("ReconstructionErrors-external-steam-achievements",0), externalSteamStatErrors=SessionState.GetInt("ReconstructionErrors-external-steam-stats",0),
            activeCanvases=Resources.FindObjectsOfTypeAll<Canvas>().Count(c=>c.gameObject.scene.IsValid() && c.isActiveAndEnabled),
            activeGraphics=Resources.FindObjectsOfTypeAll<UnityEngine.UI.Graphic>().Count(g=>g.gameObject.scene.IsValid() && g.isActiveAndEnabled)
        };
        if(timedOut) report.failures.Add("Play-mode test timed out");
        if(report.scene!="SampleScene") report.failures.Add("Expected startup scene SampleScene");
        if(report.runtimeErrors>0) report.failures.Add("Unclassified runtime errors or exceptions: "+report.runtimeErrors);
        if(!report.graphicsDeviceAvailable || report.cameras==0) report.failures.Add("No graphics device or active camera");
        if(report.activeCanvases==0 || report.activeGraphics==0) report.failures.Add("No active Canvas UI");
        report.screenshotExists=File.Exists(report.screenshot);
        if(!report.screenshotExists) report.failures.Add("Actual Game View screenshot was not produced");
        else
        {
            Texture2D image=null;
            try { image=new Texture2D(2,2); if(!image.LoadImage(File.ReadAllBytes(report.screenshot))) throw new Exception("PNG decoding failed"); report.screenshotWidth=image.width; report.screenshotHeight=image.height; if(image.width<320 || image.height<180) report.failures.Add("Game View screenshot resolution too small"); }
            catch(Exception error) { report.failures.Add("Screenshot validation: "+error.Message); }
            finally { if(image) UnityEngine.Object.DestroyImmediate(image); }
        }
        report.verdict=report.failures.Count==0 ? "PASS (scoped startup only)" : "FAIL";
        File.WriteAllText(Path.Combine(Evidence,"smoke-report.json"),JsonUtility.ToJson(report,true));
        Debug.Log("RECONSTRUCTION_SMOKE_COMPLETE verdict="+report.verdict+" runtimeErrors="+report.runtimeErrors+" excludedEditorSearch="+report.editorSearchErrors+" unavailableSteamAchievements="+report.externalSteamErrors);
        if(exitEditor) RequestSmokeExit(report.failures.Count==0 ? 0 : 5);
    }

    [Serializable] public class InteractionReport
    {
        public string verdict, persistentDataPath, buttonPath, selectorState, playingState;
        public string scope="Invoke original UI Button.onClick events: New Run to active train selector, default Select to PlayingGameState. No claim of physical input or full gameplay fidelity.";
        public bool selectorActive, playingReached, selectorScreenshot, playingScreenshot, confirmedTestRunReplacement, selectionAdvancedBeforeHarness;
        public string testSaveBackup;
        public int runtimeErrors;
        public List<string> failures=new List<string>();
    }
    static InteractionReport Interaction;
    static int InteractionStage;
    public static void MenuInteraction()
    {
        Smoke();
        SessionState.SetBool("ReconstructionInteraction",true);
    }
    static string CurrentGameState()
    {
        var manager=UnityEngine.Object.FindFirstObjectByType<Monoblok.GameManagement.GameStateManager>();
        if(!manager) return "missing GameStateManager";
        var field=manager.GetType().GetField("stateMachine",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic);
        var machine=field.GetValue(manager) as Monoblok.StateMachine.StateMachine;
        return machine?.CurrentStateType?.FullName ?? "missing state machine";
    }
    static UnityEngine.UI.Button OriginalButton(UnityEngine.Object owner,string propertyName)
    {
        using(var serialized=new SerializedObject(owner))
        {
            var button=serialized.FindProperty(propertyName)?.objectReferenceValue as UnityEngine.UI.Button;
            if(!button || !button.isActiveAndEnabled || !button.interactable) throw new InvalidOperationException("Original button not available: "+propertyName);
            return button;
        }
    }
    static void InteractionUpdate(double elapsed)
    {
        try
        {
            if(InteractionStage==0)
            {
                FinishSmoke(false,false);
                Interaction=new InteractionReport { persistentDataPath=Application.persistentDataPath };
                var startup=JsonUtility.FromJson<SmokeReport>(File.ReadAllText(Path.Combine(Evidence,"smoke-report.json")));
                if(startup.failures.Count>0) Interaction.failures.Add("Startup check failed; continuing separate interaction coverage: "+string.Join("; ",startup.failures));
                if(SessionState.GetBool("ReconstructionForceIntro",false))
                {
                    const string key="First_Time";
                    SessionState.SetBool("ReconstructionFirstTimeHadKey",PlayerPrefs.HasKey(key));
                    SessionState.SetInt("ReconstructionFirstTimePrior",PlayerPrefs.GetInt(key,0));
                    SessionState.SetBool("ReconstructionFirstTimeSaved",true);
                    PlayerPrefs.SetInt(key,0);
                }
                var menu=UnityEngine.Object.FindFirstObjectByType<Monoblok.MainMenu>();
                var button=OriginalButton(menu,"newRunButton");
                Interaction.buttonPath=GetPath(button.transform);
                button.onClick.Invoke();
                using(var serialized=new SerializedObject(menu))
                {
                    var confirmation=serialized.FindProperty("confirmNewRunModal").objectReferenceValue as Monoblok.Core.UI.YesNoModal;
                    if(confirmation && confirmation.isActiveAndEnabled)
                    {
                        string identity=Application.persistentDataPath.Replace('\\','/');
                        if(!identity.EndsWith("/UnityReconstruction/EndlessRails-Reconstructed-2022",StringComparison.Ordinal)) throw new InvalidOperationException("Refusing new-run confirmation outside isolated reconstruction save identity");
                        string backup=Path.GetFullPath(Path.Combine(Application.dataPath,"../../migration-2022/backups/test-save-before-new-run/"+DateTime.UtcNow.ToString("yyyyMMddTHHmmssfffZ")));
                        Directory.CreateDirectory(backup);
                        foreach(string file in Directory.GetFiles(Application.persistentDataPath,"*",SearchOption.AllDirectories))
                        {
                            string relative=file.Substring(Application.persistentDataPath.Length).TrimStart(Path.DirectorySeparatorChar,Path.AltDirectorySeparatorChar);
                            string destination=Path.Combine(backup,relative);
                            Directory.CreateDirectory(Path.GetDirectoryName(destination));
                            File.Copy(file,destination,false);
                        }
                        Interaction.testSaveBackup=backup;
                        OriginalButton(confirmation,"yesButton").onClick.Invoke();
                        Interaction.confirmedTestRunReplacement=true;
                        Debug.Log("RECONSTRUCTION_INTERACTION_TEST_RUN_CONFIRMED backup="+backup);
                    }
                }
                InteractionStage=1;
                Debug.Log("RECONSTRUCTION_INTERACTION_NEW_RUN_INVOKED");
            }
            if(InteractionStage==1 && elapsed>40)
            {
                var selector=UnityEngine.Object.FindFirstObjectByType<Monoblok.Train.TrainConfigSelectorView>();
                Interaction.selectorActive=selector && selector.isActiveAndEnabled;
                Interaction.selectorState=CurrentGameState();
                if(!Interaction.selectorActive || !Interaction.selectorState.EndsWith("TrainConfigSelectorState")) throw new InvalidOperationException("New Run did not reach active train selector");
                ScreenCapture.CaptureScreenshot(Path.Combine(Evidence,"interaction-selector.png"));
                InteractionStage=2;
            }
            if(InteractionStage==2 && elapsed>44)
            {
                var selector=UnityEngine.Object.FindFirstObjectByType<Monoblok.Train.TrainConfigSelectorView>();
                if(selector && selector.isActiveAndEnabled)
                {
                    OriginalButton(selector,"selectButton").onClick.Invoke();
                    InteractionStage=3;
                    Debug.Log("RECONSTRUCTION_INTERACTION_SELECT_INVOKED");
                }
                else if(CurrentGameState().EndsWith("PlayingGameState"))
                {
                    Interaction.selectionAdvancedBeforeHarness=true;
                    InteractionStage=3;
                    Debug.Log("RECONSTRUCTION_INTERACTION_SELECTION_ALREADY_ADVANCED");
                }
                else if(elapsed>75) throw new InvalidOperationException("Selector was hidden and original state did not advance to PlayingGameState");
            }
            if(InteractionStage==3 && elapsed>58)
            {
                Interaction.playingState=CurrentGameState();
                Interaction.playingReached=Interaction.playingState.EndsWith("PlayingGameState");
                if(!Interaction.playingReached) Interaction.failures.Add("Default selection did not reach PlayingGameState");
                ScreenCapture.CaptureScreenshot(Path.Combine(Evidence,"interaction-playing.png"));
                InteractionStage=4;
            }
            if(SessionState.GetBool("ReconstructionGameplay",false) && InteractionStage>=3 && elapsed>54) GameplayUpdate(elapsed);
            else if(InteractionStage==4 && elapsed>64) FinishInteraction();
        }
        catch(Exception error)
        {
            if(Interaction==null) Interaction=new InteractionReport { persistentDataPath=Application.persistentDataPath };
            Interaction.failures.Add(error.ToString());
            FinishInteraction();
        }
    }
    static void FinishInteraction()
    {
        Interaction.runtimeErrors=SessionState.GetInt("ReconstructionErrors-runtime",0);
        if(Interaction.runtimeErrors>0) Interaction.failures.Add("Runtime errors during startup/interaction: "+Interaction.runtimeErrors);
        Interaction.selectorScreenshot=File.Exists(Path.Combine(Evidence,"interaction-selector.png"));
        Interaction.playingScreenshot=File.Exists(Path.Combine(Evidence,"interaction-playing.png"));
        if(!Interaction.selectorScreenshot || !Interaction.playingScreenshot) Interaction.failures.Add("Interaction screenshot missing");
        Interaction.verdict=Interaction.failures.Count==0 ? "PASS (logical UI event transitions)" : "FAIL";
        File.WriteAllText(Path.Combine(Evidence,"interaction-report.json"),JsonUtility.ToJson(Interaction,true));
        Debug.Log("RECONSTRUCTION_INTERACTION_COMPLETE verdict="+Interaction.verdict);
        RequestSmokeExit(Interaction.failures.Count==0 ? 0 : 7);
    }

    [Serializable] public class GameplayReport
    {
        public string behaviorVerdict, runtimeVerdict;
        public bool licensedAnimationFeatureDiagnostic;
        public string verdict, scope="Original UI New Run/Select, original intro skip handler and tutorial confirm events when needed; require actual active Enemy objects and 10 seconds after first spawn. No physical-input or whole-game fidelity claim.";
        public bool gameStarted, skipHandlerInvoked, screenshotExists;
        public int maxActiveEnemies, tutorialConfirms, runtimeErrors;
        public double firstEnemyElapsed, observedAfterFirstSpawn;
        public List<string> failures=new List<string>();
    }
    static GameplayReport Gameplay;
    static double NextGameplayUiAction;
    public static void GameplaySmoke()
    {
        MenuInteraction();
        SessionState.SetBool("ReconstructionGameplay",true);
        NextGameplayUiAction=0;
    }
    public static void IntroTeardownSmoke()
    {
        MenuInteraction();
        SessionState.SetBool("ReconstructionForceIntro",true);
    }
    static void GameplayUpdate(double elapsed)
    {
        if(Gameplay==null) Gameplay=new GameplayReport();
        var manager=UnityEngine.Object.FindFirstObjectByType<Monoblok.GameManagement.GameManager>();
        var progressField=manager?.GetType().GetField("gameInProgress",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic);
        Gameplay.gameStarted=manager && progressField!=null && (bool)progressField.GetValue(manager);
        if(!Gameplay.gameStarted && elapsed>=NextGameplayUiAction)
        {
            NextGameplayUiAction=elapsed+1;
            var cutscene=UnityEngine.Object.FindFirstObjectByType<Monoblok.CutscenePlayer>();
            if(cutscene && !Gameplay.skipHandlerInvoked)
            {
                var directorField=cutscene.GetType().GetField("currentDirector",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic);
                var director=directorField.GetValue(cutscene) as UnityEngine.Playables.PlayableDirector;
                if(director && director.state==UnityEngine.Playables.PlayState.Playing)
                {
                    cutscene.GetType().GetMethod("HandleSkipPressed",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).Invoke(cutscene,null);
                    Gameplay.skipHandlerInvoked=true;
                }
            }
            var modal=UnityEngine.Object.FindAnyObjectByType<Monoblok.Core.UI.OneButtonModal>();
            if(modal && modal.isActiveAndEnabled)
            {
                OriginalButton(modal,"confirmButton").onClick.Invoke();
                Gameplay.tutorialConfirms++;
            }
        }
        int enemies=Resources.FindObjectsOfTypeAll<Monoblok.Enemy.Enemy>().Count(enemy=>enemy.gameObject.scene.IsValid() && enemy.isActiveAndEnabled && !enemy.IsDead);
        Gameplay.maxActiveEnemies=Math.Max(Gameplay.maxActiveEnemies,enemies);
        if(enemies>0 && Gameplay.firstEnemyElapsed==0) Gameplay.firstEnemyElapsed=elapsed;
        Gameplay.observedAfterFirstSpawn=Gameplay.firstEnemyElapsed>0 ? elapsed-Gameplay.firstEnemyElapsed : 0;
        if(Gameplay.observedAfterFirstSpawn<10 && elapsed<135) return;
        Gameplay.failures.Clear();
        if(!Gameplay.gameStarted) Gameplay.failures.Add("Original game-in-progress flag did not become true");
        if(Gameplay.maxActiveEnemies==0 || Gameplay.observedAfterFirstSpawn<10) Gameplay.failures.Add("Did not observe actual active enemies for 10 seconds after first spawn");
        Gameplay.runtimeErrors=SessionState.GetInt("ReconstructionErrors-runtime",0);
        if(Gameplay.runtimeErrors>0) Gameplay.failures.Add("Runtime errors during gameplay: "+Gameplay.runtimeErrors);
        string screenshot=Path.Combine(Evidence,"gameplay-wave1.png");
        if(!SessionState.GetBool("ReconstructionGameplayCaptureRequested",false))
        {
            SessionState.SetBool("ReconstructionGameplayCaptureRequested",true);
            SessionState.SetString("ReconstructionGameplayCaptureAt",DateTime.UtcNow.ToString("O"));
            ScreenCapture.CaptureScreenshot(screenshot);
            return;
        }
        if((DateTime.UtcNow-DateTime.Parse(SessionState.GetString("ReconstructionGameplayCaptureAt",DateTime.UtcNow.ToString("O"))).ToUniversalTime()).TotalSeconds<3) return;
        Gameplay.screenshotExists=File.Exists(screenshot);
        if(!Gameplay.screenshotExists) Gameplay.failures.Add("Gameplay screenshot missing");
        Gameplay.behaviorVerdict=Gameplay.gameStarted && Gameplay.maxActiveEnemies>0 && Gameplay.observedAfterFirstSpawn>=10 ? "PASS (first enemy spawn + 10 seconds)" : "FAIL";
        Gameplay.runtimeVerdict=Gameplay.runtimeErrors==0 ? "PASS" : "FAIL";
        Gameplay.licensedAnimationFeatureDiagnostic=File.ReadAllText(Path.Combine(Evidence,"smoke-errors.log")).Contains("Animancer Pro-Only Feature Used: Animator Controllers");
        Gameplay.verdict=Gameplay.failures.Count==0 ? "PASS (first enemy spawn + 10 seconds)" : "FAIL";
        File.WriteAllText(Path.Combine(Evidence,"gameplay-report.json"),JsonUtility.ToJson(Gameplay,true));
        if(Gameplay.failures.Count>0) Interaction.failures.Add("First-wave gameplay check failed; see gameplay-report.json");
        Debug.Log("RECONSTRUCTION_GAMEPLAY_COMPLETE verdict="+Gameplay.verdict+" maxActiveEnemies="+Gameplay.maxActiveEnemies);
        FinishInteraction();
    }

    [Serializable] public class ShutdownReport
    {
        public string verdict;
        public bool playModeExited;
        public int runtimeErrorsBeforeShutdown, runtimeErrorsAfterShutdown, shutdownRuntimeErrors, exitCode;
    }
    static void RequestSmokeExit(int exitCode)
    {
        SessionState.SetInt("ReconstructionRequestedExitCode",exitCode);
        SessionState.SetInt("ReconstructionErrorsBeforeShutdown",SessionState.GetInt("ReconstructionErrors-runtime",0));
        SessionState.SetString("ReconstructionShutdownStarted",DateTime.UtcNow.ToString("O"));
        SessionState.SetBool("ReconstructionShutdownPending",true);
        if(EditorApplication.isPlayingOrWillChangePlaymode) EditorApplication.ExitPlaymode();
    }
    static void CompleteSmokeShutdown()
    {
        bool exited=!EditorApplication.isPlayingOrWillChangePlaymode;
        var started=DateTime.Parse(SessionState.GetString("ReconstructionShutdownStarted",DateTime.UtcNow.ToString("O"))).ToUniversalTime();
        if(!exited && (DateTime.UtcNow-started).TotalSeconds<30) return;
        if(exited)
        {
            string settled=SessionState.GetString("ReconstructionEnteredEditModeSince","");
            if(settled=="") { SessionState.SetString("ReconstructionEnteredEditModeSince",DateTime.UtcNow.ToString("O")); return; }
            if((DateTime.UtcNow-DateTime.Parse(settled).ToUniversalTime()).TotalSeconds<3) return;
        }
        var report=new ShutdownReport {
            playModeExited=exited, runtimeErrorsBeforeShutdown=SessionState.GetInt("ReconstructionErrorsBeforeShutdown",0),
            runtimeErrorsAfterShutdown=SessionState.GetInt("ReconstructionErrors-runtime",0), exitCode=SessionState.GetInt("ReconstructionRequestedExitCode",5)
        };
        report.shutdownRuntimeErrors=report.runtimeErrorsAfterShutdown-report.runtimeErrorsBeforeShutdown;
        report.verdict=exited && report.shutdownRuntimeErrors==0 ? "PASS (play-mode teardown)" : "FAIL";
        if(report.verdict=="FAIL") report.exitCode=8;
        if(SessionState.GetBool("ReconstructionFirstTimeSaved",false))
        {
            if(SessionState.GetBool("ReconstructionFirstTimeHadKey",false)) PlayerPrefs.SetInt("First_Time",SessionState.GetInt("ReconstructionFirstTimePrior",0));
            else PlayerPrefs.DeleteKey("First_Time");
            PlayerPrefs.Save();
            SessionState.SetBool("ReconstructionFirstTimeSaved",false);
        }
        File.WriteAllText(Path.Combine(Evidence,"smoke-shutdown-report.json"),JsonUtility.ToJson(report,true));
        SessionState.SetBool("ReconstructionSmoke",false);
        SessionState.SetBool("ReconstructionShutdownPending",false);
        Debug.Log("RECONSTRUCTION_SMOKE_SHUTDOWN verdict="+report.verdict+" runtimeErrors="+report.shutdownRuntimeErrors);
        EditorApplication.Exit(report.exitCode);
    }

    [Serializable] public class OriginalLightField { public string property; public float value; }
    [Serializable] public class OriginalLightEvidence { public string destination, destination_component_local_id; public OriginalLightField[] fields; }
    [Serializable] public class LightRepairReport { public string asset, componentLocalId; public List<string> before=new List<string>(); public List<string> after=new List<string>(); }
    public static void RestoreOriginalLightFields()
    {
        string evidencePath=Path.GetFullPath(Path.Combine(Application.dataPath,"../../coordinator/verification/original-additional-light-fields.json"));
        var evidence=JsonUtility.FromJson<OriginalLightEvidence>(File.ReadAllText(evidencePath));
        string originalPrefab=File.ReadAllText(evidence.destination);
        var root=AssetDatabase.LoadAssetAtPath<GameObject>(evidence.destination);
        var component=root.GetComponentsInChildren<UnityEngine.Rendering.Universal.UniversalAdditionalLightData>(true).Single(c=> {
            string guid; long localId; AssetDatabase.TryGetGUIDAndLocalFileIdentifier(c,out guid,out localId); return localId.ToString()==evidence.destination_component_local_id;
        });
        var report=new LightRepairReport { asset=evidence.destination, componentLocalId=evidence.destination_component_local_id };
        using(var serialized=new SerializedObject(component))
        {
            foreach(var field in evidence.fields)
            {
                var property=serialized.FindProperty(field.property);
                if(property==null) throw new InvalidOperationException("Original light field not found: "+field.property);
                report.before.Add(field.property+"="+LightFieldValue(property));
                if(property.propertyType==SerializedPropertyType.Boolean) property.boolValue=field.value!=0;
                else if(property.propertyType==SerializedPropertyType.Float) property.floatValue=field.value;
                else property.longValue=(long)field.value;
            }
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }
        EditorUtility.SetDirty(component);
        AssetDatabase.SaveAssets();
        // Unity normalizes unrelated prefab records on save. Retain only the exact
        // component record generated by its serializer; leave every other byte intact.
        string pattern=@"(?ms)^--- !u!114 &"+Regex.Escape(evidence.destination_component_local_id)+@"\r?\n.*?(?=^--- |\z)";
        var generated=Regex.Match(File.ReadAllText(evidence.destination),pattern);
        if(!generated.Success || Regex.Matches(originalPrefab,pattern).Count!=1) throw new InvalidOperationException("Original light component record was not unique");
        File.WriteAllText(evidence.destination,Regex.Replace(originalPrefab,pattern,m=>generated.Value));
        AssetDatabase.ImportAsset(evidence.destination,ImportAssetOptions.ForceUpdate);
        // Verify a separately deserialized prefab, rather than the object just edited.
        var loaded=PrefabUtility.LoadPrefabContents(evidence.destination);
        try
        {
            var actual=loaded.GetComponentsInChildren<UnityEngine.Rendering.Universal.UniversalAdditionalLightData>(true).Single();
            using(var serialized=new SerializedObject(actual))
                foreach(var field in evidence.fields)
                {
                    var property=serialized.FindProperty(field.property);
                    float value=property.propertyType==SerializedPropertyType.Boolean ? (property.boolValue ? 1 : 0) : property.propertyType==SerializedPropertyType.Float ? property.floatValue : property.longValue;
                    report.after.Add(field.property+"="+LightFieldValue(property));
                    if(value!=field.value) throw new InvalidOperationException("Original light readback mismatch: "+field.property+" expected="+field.value+" actual="+value);
                }
        }
        finally { PrefabUtility.UnloadPrefabContents(loaded); }
        File.WriteAllText(Path.Combine(Evidence,"original-light-fields-repair.json"),JsonUtility.ToJson(report,true));
        Debug.Log("RECONSTRUCTION_LIGHT_FIELDS_RESTORED count="+evidence.fields.Length);
        Audit();
    }
    static string LightFieldValue(SerializedProperty property)
    {
        if(property.propertyType==SerializedPropertyType.Boolean) return property.boolValue.ToString();
        if(property.propertyType==SerializedPropertyType.Float) return property.floatValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
        return property.longValue.ToString();
    }

    public static bool DiagnosticBuildActive { get; private set; }
    public static int TerrainVariantsStripped, FinalPostVariantsStripped, FinalPostVariantsKept, FinalPostXRVariantsStripped, UberPostVariantsStripped, UberPostVariantsKept, UberPostXRVariantsStripped;
    [Serializable] public class OriginalVariantPass { public string name; public string[] fragmentKeywordSets; }
    [Serializable] public class OriginalVariantShader { public string shader; public OriginalVariantPass[] passes; }
    [Serializable] public class OriginalVariantPolicy { public OriginalVariantShader[] shaders; }
    [Serializable] public class VariantRow { public string shader, pass; public int before, kept, removed; public string[] keptKeywordSets; }
    [Serializable] public class VariantReport { public List<VariantRow> rows=new List<VariantRow>(); }
    public static Dictionary<string, Dictionary<string,HashSet<string>>> DiagnosticPostPolicy;
    public static VariantReport PostVariantReport;
    static void LoadDiagnosticPostPolicy()
    {
        string path=Path.GetFullPath(Path.Combine(Application.dataPath,"../../coordinator/build-review/original-postprocessing-variants.json"));
        var policy=JsonUtility.FromJson<OriginalVariantPolicy>(File.ReadAllText(path));
        DiagnosticPostPolicy=policy.shaders.ToDictionary(shader=>shader.shader,shader=>shader.passes.ToDictionary(pass=>pass.name,pass=>new HashSet<string>(pass.fragmentKeywordSets,StringComparer.Ordinal)),StringComparer.Ordinal);
        if(DiagnosticPostPolicy["Hidden/Universal Render Pipeline/FinalPost"]["FinalPost"].Count!=48 || DiagnosticPostPolicy["Hidden/Universal Render Pipeline/UberPost"]["UberPost"].Count!=720) throw new InvalidOperationException("Original post-processing variant coverage does not match source evidence");
        PostVariantReport=new VariantReport();
    }
    public static void RecordPostVariantRow(VariantRow row)
    {
        PostVariantReport.rows.Add(row);
        File.WriteAllText(Path.Combine(Evidence,"diagnostic-postprocess-variants.json"),JsonUtility.ToJson(PostVariantReport,true));
    }
    public static readonly HashSet<string> FinalPostKeptSets = new HashSet<string>(StringComparer.Ordinal);
    public static void BuildPlayer() { BuildPlayerInternal(false); }
    public static void BuildPlayerDiagnostic() { BuildPlayerInternal(true); }
    static void BuildPlayerInternal(bool diagnostic)
    {
        Directory.CreateDirectory(Evidence);
        string output=Path.GetFullPath(Path.Combine(Application.dataPath,diagnostic ? "../../migration-2022/player-diagnostic/EndlessRails-2022.exe" : "../../migration-2022/player/EndlessRails-2022.exe"));
        var report=new PlayerBuildReport { output=output, scope=diagnostic ? "Windows x64 development, DX11 only; strip only Hidden/TerrainEngine/* variants because all 13992 original objects and recovered source contain no Terrain system. FinalPost/UberPost fragments retain exactly the original48/720 keyword sets; absent FinalPostXR/UberPostXR passes stripped. Other shader families unchanged." : "Windows x64 release, original project graphics settings and no custom variant stripping; release build (no development player connection) for offline use" };
        var target=BuildTarget.StandaloneWindows64;
        bool priorDefault=PlayerSettings.GetUseDefaultGraphicsAPIs(target);
        var priorApis=PlayerSettings.GetGraphicsAPIs(target);
        int exitCode=6;
        TerrainVariantsStripped=0; FinalPostVariantsStripped=0; FinalPostVariantsKept=0; FinalPostXRVariantsStripped=0; FinalPostKeptSets.Clear();
        UberPostVariantsStripped=0; UberPostVariantsKept=0; UberPostXRVariantsStripped=0;
        if(diagnostic) LoadDiagnosticPostPolicy();
        try
        {
            DiagnosticBuildActive=diagnostic;
            if(diagnostic)
            {
                PlayerSettings.SetUseDefaultGraphicsAPIs(target,false);
                PlayerSettings.SetGraphicsAPIs(target,new[]{UnityEngine.Rendering.GraphicsDeviceType.Direct3D11});
            }
            var result=UnityEditor.BuildPipeline.BuildPlayer(new UnityEditor.BuildPlayerOptions {
                scenes=new[]{"Assets/Scenes/Bootstrap.unity","Assets/Scenes/SampleScene.unity"}, locationPathName=output,
                target=target, options=diagnostic ? BuildOptions.Development : BuildOptions.None
            });
            report.result=result.summary.result.ToString(); report.errors=result.summary.totalErrors; report.warnings=result.summary.totalWarnings;
            exitCode=result.summary.result==UnityEditor.Build.Reporting.BuildResult.Succeeded ? 0 : 6;
        }
        catch(Exception error) { report.result="Exception"; report.exception=error.ToString(); Debug.LogException(error); }
        finally
        {
            DiagnosticBuildActive=false;
            if(diagnostic)
            {
                PlayerSettings.SetGraphicsAPIs(target,priorApis);
                PlayerSettings.SetUseDefaultGraphicsAPIs(target,priorDefault);
            }
            report.graphicsSettingsRestored=PlayerSettings.GetUseDefaultGraphicsAPIs(target)==priorDefault && PlayerSettings.GetGraphicsAPIs(target).SequenceEqual(priorApis);
            report.terrainVariantsStripped=TerrainVariantsStripped;
            report.finalPostVariantsStripped=FinalPostVariantsStripped; report.finalPostVariantsKept=FinalPostVariantsKept; report.finalPostXRVariantsStripped=FinalPostXRVariantsStripped;
            report.uberPostVariantsStripped=UberPostVariantsStripped; report.uberPostVariantsKept=UberPostVariantsKept; report.uberPostXRVariantsStripped=UberPostXRVariantsStripped;
            File.WriteAllLines(Path.Combine(Evidence,"diagnostic-finalpost-kept-keyword-sets.txt"),FinalPostKeptSets.OrderBy(v=>v,StringComparer.Ordinal));
            File.WriteAllText(Path.Combine(Evidence,"player-build.json"),JsonUtility.ToJson(report,true));
        }
        Debug.Log("RECONSTRUCTION_BUILD_COMPLETE result="+report.result+" errors="+report.errors+" terrainVariantsStripped="+report.terrainVariantsStripped);
        EditorApplication.Exit(exitCode);
    }

    public static void RemapAndAudit()
    {
        var original = JsonUtility.FromJson<Report>(File.ReadAllText(Path.Combine(Evidence,"unity-audit-original-libraries.json")));
        var desired = original.scripts.Where(s => !string.IsNullOrEmpty(s.type) && (s.assetPath.EndsWith("/UnityEngine.UI.dll") || s.assetPath.EndsWith("/Unity.TextMeshPro.dll") || s.assetPath.EndsWith("/Unity.InputSystem.dll"))).ToList();
        var current = MonoImporter.GetAllRuntimeMonoScripts().Where(s=>s.GetClass()!=null).GroupBy(s=>s.GetClass().Assembly.GetName().Name+":"+s.GetClass().FullName).ToDictionary(g=>g.Key,g=>g.First());
        var replacements = new Dictionary<string, string>();
        var mapped = new List<ScriptRow>();
        foreach (var old in desired)
        {
            MonoScript script;
            if (!current.TryGetValue(old.assembly+":"+old.type,out script)) { Debug.LogError("Unmapped migrated type: "+old.type); continue; }
            string guid; long localId;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(script,out guid,out localId);
            replacements[old.guid+":"+old.localId]="{fileID: "+localId+", guid: "+guid+", type: 3}";
            mapped.Add(new ScriptRow { assetPath=AssetDatabase.GetAssetPath(script),guid=guid,localId=localId.ToString(),type=old.type,assembly=old.assembly });
        }
        var pattern = new Regex(@"\{fileID: (-?\d+), guid: ([0-9a-f]{32}), type: 3\}");
        int count=0;
        foreach (var path in Directory.GetFiles(Application.dataPath,"*",SearchOption.AllDirectories).Where(p=>new[]{".unity",".prefab",".asset",".mat",".controller",".anim",".playable"}.Contains(Path.GetExtension(p))))
        {
            string before=File.ReadAllText(path);
            string after=pattern.Replace(before,m=> { string target; if (replacements.TryGetValue(m.Groups[2].Value+":"+m.Groups[1].Value,out target)) { count++; return target; } return m.Value; });
            if(before!=after) File.WriteAllText(path,after);
        }
        File.WriteAllText(Path.Combine(Evidence,"package-script-remapping.json"),JsonUtility.ToJson(new Report { scripts=mapped },true));
        Debug.Log("PACKAGE_SCRIPT_REMAP references="+count+" types="+mapped.Count);
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        ConfigureAndAudit();
    }

    public static void Audit()
    {
        Directory.CreateDirectory(Evidence);
        var report = new Report { unityVersion = Application.unityVersion };
        ActiveAuditReport = report;
        try
        {
            // The recovered player Localization assembly has no editor asset discovery hook.
            // Load its original preloaded settings before edit-mode LocalizeStringEvent.OnEnable.
            var localization = AssetDatabase.LoadAssetAtPath<UnityEngine.Localization.Settings.LocalizationSettings>("Assets/MonoBehaviour/Localization Settings.asset");
            if (!localization) throw new InvalidOperationException("Original Localization Settings asset did not load");
            UnityEngine.Localization.Settings.LocalizationSettings.Instance = localization;
            foreach (var script in MonoImporter.GetAllRuntimeMonoScripts())
            {
                var type = script.GetClass();
                string guid; long localId;
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(script, out guid, out localId);
                report.scripts.Add(new ScriptRow { assetPath=AssetDatabase.GetAssetPath(script), guid=guid, localId=localId.ToString(), type=type?.FullName, assembly=type?.Assembly.GetName().Name });
            }
            foreach (var scenePath in new[] { "Assets/Scenes/Bootstrap.unity", "Assets/Scenes/SampleScene.unity" })
            {
                var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                foreach (var root in scene.GetRootGameObjects())
                    Visit(root.transform, scenePath, report);
            }
            foreach (var guid in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" }))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                try
                {
                    var root = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (root == null) { report.issues.Add(new AssetIssue { assetPath=path, error="Prefab failed to load" }); continue; }
                    report.prefabs++;
                    foreach (var transform in root.GetComponentsInChildren<Transform>(true))
                        {
                            report.missingPrefabScripts += GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(transform.gameObject);
                            foreach(var component in transform.GetComponents<Component>()) if(component) AuditReferences(component,path+":"+GetPath(transform),report);
                        }
                }
                catch (Exception error) { report.issues.Add(new AssetIssue { assetPath=path, error=error.ToString() }); }
            }
            foreach (var path in AssetDatabase.GetAllAssetPaths().Where(p => p.StartsWith("Assets/") && new[]{".asset",".playable",".controller",".overrideController",".anim",".mat",".renderTexture"}.Contains(Path.GetExtension(p))))
            {
                try
                {
                    var asset = AssetDatabase.LoadMainAssetAtPath(path);
                    if (asset == null) report.issues.Add(new AssetIssue { assetPath=path, error="Asset failed to load" });
                    else
                    {
                        report.serializedAssetFiles++;
                        if (asset is ScriptableObject) report.scriptableAssets++;
                        foreach(var subAsset in AssetDatabase.LoadAllAssetsAtPath(path)) if(subAsset)
                        {
                            report.serializedSubAssets++;
                            AuditReferences(subAsset,path+":"+subAsset.name,report);
                        }
                    }
                }
                catch (Exception error) { report.issues.Add(new AssetIssue { assetPath=path, error=error.ToString() }); }
            }
            File.WriteAllText(Path.Combine(Evidence, "unity-audit.json"), JsonUtility.ToJson(report, true));
            Debug.Log($"RECONSTRUCTION_AUDIT_COMPLETE objects={report.objects.Count} missingSceneScripts={report.missingSceneScripts} missingPrefabScripts={report.missingPrefabScripts}");
            EditorApplication.Exit(report.missingSceneScripts == 0 && report.missingPrefabScripts == 0 && report.issues.Count == 0 && report.missingObjectReferences.Count == 0 && report.loggedErrors.Count == 0 ? 0 : 2);
        }
        catch (Exception error)
        {
            File.WriteAllText(Path.Combine(Evidence,"unity-audit-failure.txt"),error.ToString());
            Debug.LogException(error);
            EditorApplication.Exit(3);
        }
        finally { ActiveAuditReport = null; }
    }

    static void Visit(Transform transform, string scene, Report report)
    {
        var go = transform.gameObject;
        long localId; string guid;
        AssetDatabase.TryGetGUIDAndLocalFileIdentifier(go, out guid, out localId);
        int missing = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
        report.missingSceneScripts += missing;
        report.objects.Add(new ObjectRow { scene=scene, path=GetPath(transform), name=go.name, sourceId=localId.ToString(), sibling=transform.GetSiblingIndex(), layer=go.layer, active=go.activeSelf, localPosition=transform.localPosition, localScale=transform.localScale, localRotation=transform.localRotation, components=go.GetComponents<Component>().Select(c=>c ? c.GetType().AssemblyQualifiedName : "MISSING").ToArray(), missingScripts=missing });
        foreach (var component in go.GetComponents<Component>()) if(component) AuditReferences(component,scene+":"+GetPath(transform),report);
        foreach (Transform child in transform) Visit(child, scene, report);
    }
    static void AuditReferences(UnityEngine.Object asset, string owner, Report report)
    {
        try
        {
            using(var serialized=new SerializedObject(asset))
            {
                var property=serialized.GetIterator();
                while(property.Next(true)) if(property.propertyType==SerializedPropertyType.ObjectReference)
                {
                    report.objectReferencesChecked++;
                    if(property.objectReferenceValue==null && property.objectReferenceInstanceIDValue!=0)
                        report.missingObjectReferences.Add(new ReferenceIssue { owner=owner+" ["+asset.GetType().FullName+"]",property=property.propertyPath,entityId=property.objectReferenceInstanceIDValue.ToString() });
                }
            }
        }
        catch(Exception error) { report.issues.Add(new AssetIssue { assetPath=owner,error="Reference audit: "+error }); }
    }
    static string GetPath(Transform transform) => transform.parent ? GetPath(transform.parent)+"/"+transform.name : transform.name;
}

// Active only for the explicit diagnostic entry point; ordinary editor/player builds
// retain all shader variants. Evidence: coordinator/build-review/terrain-use-evidence.json.
public sealed class ReconstructionTerrainDiagnosticStripper : UnityEditor.Build.IPreprocessShaders
{
    public int callbackOrder => 0;
    public void OnProcessShader(Shader shader, UnityEditor.Rendering.ShaderSnippetData snippet, IList<UnityEditor.Rendering.ShaderCompilerData> variants)
    {
        if(!ReconstructionTools.DiagnosticBuildActive) return;
        if(shader.name.StartsWith("Hidden/TerrainEngine/",StringComparison.Ordinal))
        {
            ReconstructionTools.TerrainVariantsStripped+=variants.Count;
            variants.Clear(); return;
        }
        Dictionary<string,HashSet<string>> passes;
        if(!ReconstructionTools.DiagnosticPostPolicy.TryGetValue(shader.name,out passes)) return;
        bool finalPost=shader.name=="Hidden/Universal Render Pipeline/FinalPost";
        if(snippet.passName=="FinalPostXR" || snippet.passName=="UberPostXR")
        {
            int removed=variants.Count;
            if(finalPost) ReconstructionTools.FinalPostXRVariantsStripped+=removed; else ReconstructionTools.UberPostXRVariantsStripped+=removed;
            variants.Clear();
            ReconstructionTools.RecordPostVariantRow(new ReconstructionTools.VariantRow { shader=shader.name,pass=snippet.passName+" "+snippet.shaderType,before=removed,kept=0,removed=removed,keptKeywordSets=new string[0] });
            return;
        }
        HashSet<string> originalSets;
        if(snippet.shaderType!=UnityEditor.Rendering.ShaderType.Fragment || !passes.TryGetValue(snippet.passName,out originalSets)) return;
        int before=variants.Count;
        var keptSets=new HashSet<string>(StringComparer.Ordinal);
        for(int index=variants.Count-1;index>=0;index--)
        {
            string keywords=string.Join(";",variants[index].shaderKeywordSet.GetShaderKeywords().Select(k=>k.name).OrderBy(k=>k,StringComparer.Ordinal));
            if(!originalSets.Contains(keywords)) variants.RemoveAt(index);
            else { keptSets.Add(keywords); if(finalPost) ReconstructionTools.FinalPostKeptSets.Add(keywords); }
        }
        if(finalPost) { ReconstructionTools.FinalPostVariantsStripped+=before-variants.Count; ReconstructionTools.FinalPostVariantsKept+=variants.Count; }
        else { ReconstructionTools.UberPostVariantsStripped+=before-variants.Count; ReconstructionTools.UberPostVariantsKept+=variants.Count; }
        if(before>0 && variants.Count==0) throw new UnityEditor.Build.BuildFailedException("Exact original post-processing variant policy retained zero variants for "+shader.name);
        ReconstructionTools.RecordPostVariantRow(new ReconstructionTools.VariantRow { shader=shader.name,pass=snippet.passName,before=before,kept=variants.Count,removed=before-variants.Count,keptKeywordSets=keptSets.OrderBy(k=>k,StringComparer.Ordinal).ToArray() });
        Debug.Log("RECONSTRUCTION_POST_VARIANTS shader="+shader.name+" pass="+snippet.passName+" before="+before+" kept="+variants.Count+" removed="+(before-variants.Count));
    }
}
