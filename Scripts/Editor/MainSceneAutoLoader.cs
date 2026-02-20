using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Game.Editor
{
    [InitializeOnLoad]
    public static class MainSceneAutoLoader
    {
        private const string MainScenePath = "Assets/Game/Scenes/Main/Main_menu.unity";
        private const string PrefkeyPrevScene = "PREVIOUS_SCENE";
        private const string AutoLoaderEnabledKey = "MAIN_SCENE_AUTO_LOADER_ENABLED";

        static MainSceneAutoLoader()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        [MenuItem("Tools/Auto Load Main Scene On Play")]
        private static void ToggleAutoLoader()
        {
            bool currentState = EditorPrefs.GetBool(AutoLoaderEnabledKey, true);
            EditorPrefs.SetBool(AutoLoaderEnabledKey, !currentState);
            Menu.SetChecked("Tools/Auto Load Main Scene On Play", !currentState);
        }

        [MenuItem("Tools/Auto Load Main Scene On Play", true)]
        private static bool ToggleAutoLoaderValidate()
        {
            bool currentState = EditorPrefs.GetBool(AutoLoaderEnabledKey, true);
            Menu.SetChecked("Tools/Auto Load Main Scene On Play", currentState);
            return true;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            if (!EditorPrefs.GetBool(AutoLoaderEnabledKey, true))
                return;

            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                    return;

                string path = EditorSceneManager.GetActiveScene().path;

                EditorPrefs.SetString(PrefkeyPrevScene, path);

                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    try
                    {
                        EditorSceneManager.OpenScene(MainScenePath);
                    }
                    catch
                    {
                        Debug.LogError($"Cannot load scene {MainScenePath}");
                        EditorApplication.isPlaying = false;
                    }
                }
                else
                {
                    EditorApplication.isPlaying = false;
                }
            }

            if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                string path = EditorPrefs.GetString(PrefkeyPrevScene);
                if (string.IsNullOrEmpty(path))
                    return;

                try
                {
                    EditorSceneManager.OpenScene(path);
                }
                catch
                {
                    Debug.LogError($"Cannot load scene {path}");
                }
            }
        }
    }
}