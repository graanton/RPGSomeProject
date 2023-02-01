using UnityEditor;
using UnityEngine;

namespace CustomTilemap3d
{
    [CustomEditor(typeof(Tilemap3D))]
    public class Tilemap3DEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Tilemap3D tilemap3d = (Tilemap3D)target;

            if (GUILayout.Button(nameof(tilemap3d.ResetGridList)))
            {
                tilemap3d.ResetGridList();
            }
            if (GUILayout.Button(nameof(SaveChanges)))
            {
                SaveChanges();
            }
        }
    }
}