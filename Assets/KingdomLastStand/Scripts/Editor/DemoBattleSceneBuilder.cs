using System.IO;
using KingdomLastStand.Demo;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KingdomLastStand.Editor
{
    public static class DemoBattleSceneBuilder
    {
        private const string ScenePath = "Assets/KingdomLastStand/Scenes/DemoBattle.unity";

        [MenuItem("Kingdom Last Stand/Create Demo Battle Scene")]
        public static void CreateDemoBattleScene()
        {
            Directory.CreateDirectory("Assets/KingdomLastStand/Scenes");
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var bootstrap = new GameObject("Kingdom Last Stand Demo");
            bootstrap.AddComponent<DemoBattleBootstrap>();
            EditorSceneManager.MarkSceneDirty(scene);

            if (!EditorSceneManager.SaveScene(scene, ScenePath))
            {
                Debug.LogError("Could not save the demo battle scene.");
                return;
            }

            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.defaultScreenWidth = 1080;
            PlayerSettings.defaultScreenHeight = 1920;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Demo scene created. Press Play to recruit units, start a wave, and drag matching archers together to merge.");
        }
    }
}
