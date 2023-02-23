using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Room), true)]
public class RoomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Room room = (Room)target;

        if (GUILayout.Button(nameof(room.TilesRectToLocalZeroPosition)))
        {
            room.TilesRectToLocalZeroPosition();
            UpdatePrefab();
        }

        if (GUILayout.Button(nameof(room.BoundsInit)))
        {
            room.BoundsInit();
            UpdatePrefab();
        }
        
        if (GUILayout.Button(nameof(room.CorrectPosition)))
        {
            room.CorrectPosition();
            UpdatePrefab();
        }
    }

    public static void UpdatePrefab()
    {
#if UNITY_EDITOR

        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null)
        {
            EditorSceneManager.MarkSceneDirty(prefabStage.scene);
        }
#endif
    }
}