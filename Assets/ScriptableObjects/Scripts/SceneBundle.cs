using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EasyButtons;

[CreateAssetMenu(fileName = "SceneBundle", menuName = "ScriptableObjects/SceneBundle", order = 1)]
public class SceneBundle : ScriptableObject
{
#if (UNITY_EDITOR)


    public SceneAsset[] m_IncludedScenes;
    [Button]
    public void SetEditorBuildSettingsScenes()
    {
        // Find valid Scene paths and make a list of EditorBuildSettingsScene
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        foreach (var sceneAsset in m_IncludedScenes)
        {
            string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
            if (!string.IsNullOrEmpty(scenePath))
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
        }

        // Set the Build Settings window Scene list
        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
    }

#endif
}
