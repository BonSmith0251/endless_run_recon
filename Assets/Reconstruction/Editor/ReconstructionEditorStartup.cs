using UnityEditor;
using UnityEditor.SceneManagement;

// Bootstrap.Initialize synchronously loads Bootstrap BeforeSceneLoad. Starting Play from
// SampleScene lets that hook destroy the scene while the editor is still processing it,
// so the editor starts Play from Bootstrap and keeps the user's open scene.
// (The Unity 6 reconstruction also re-bound player-DLL Localization and UGUI hooks here;
// the 2022 project uses the real com.unity.localization and com.unity.ugui packages.)
[InitializeOnLoad]
public static class ReconstructionEditorStartup
{
    static ReconstructionEditorStartup()
    {
        EditorApplication.delayCall += Configure;
    }

    public static void Configure()
    {
        if (EditorApplication.isCompiling || EditorApplication.isUpdating)
        {
            EditorApplication.delayCall -= Configure;
            EditorApplication.delayCall += Configure;
            return;
        }
        var bootstrap = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/Bootstrap.unity");
        if (bootstrap) EditorSceneManager.playModeStartScene = bootstrap;
    }
}
