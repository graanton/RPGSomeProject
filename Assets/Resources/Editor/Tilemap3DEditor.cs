using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tilemap3DEditor : EditorWindow
{
    [SerializeField] private GameObject _gridTilePrefab;

    private int _selectedToolbarIndex = -1;
    private int _selectedTilemap3dIndex = -1;
    private Tilemap3D _selectedTilemap3d;
    private string[] _tools = new string[2] { "Brush", "None" };
    private List<GameObject> _grid;
    private Transform _gridRoot;

    [MenuItem("Window/Tilemap3D")]
    private static void ShowWindow()
    {
        GetWindow<Tilemap3DEditor>().Show();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        switch (_selectedToolbarIndex)
        {
            case 0:
                if (_selectedTilemap3d)
                {
                    Camera camera = ((SceneView)SceneView.sceneViews[0]).camera;
                    Ray ray = camera.ScreenPointToRay(Event.current.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {

                    }
                }
                break;
            case 1:
                break;
        }
        
        
    }

    [System.Obsolete]
    private void OnGUI()
    {
        var tileMaps3d = FindObjectsOfType<Tilemap3D>();
        var tileMaps3dNames = new string[tileMaps3d.Length];

        for (int i = 0; i < tileMaps3dNames.Length; i++) 
        {
            tileMaps3dNames[i] = tileMaps3d[i].name;
        }

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Selected Tilemap3D");
        _selectedTilemap3dIndex = EditorGUILayout.Popup(_selectedTilemap3dIndex, tileMaps3dNames);

        GUILayout.EndHorizontal();
        
        if (_selectedTilemap3dIndex >= 0 && tileMaps3d[_selectedTilemap3dIndex])
        {
            _selectedTilemap3d = tileMaps3d[_selectedTilemap3dIndex];
            GUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Grid tile prefab");
            _gridTilePrefab = (GameObject)EditorGUILayout.ObjectField(_gridTilePrefab, typeof(GameObject));

            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Grid root");
            _gridRoot = (Transform)EditorGUILayout.ObjectField(_gridRoot, typeof(Transform));

            GUILayout.EndHorizontal();
            
            if (_grid.Count == 0)
            {
                if (GUILayout.Button("Create grid"))
                {
                    for (int x = 0; x < _selectedTilemap3d.size.x; x++)
                    {
                        for (int y = 0; y < _selectedTilemap3d.size.y; y++)
                        {
                            _grid.Add(Instantiate(_gridTilePrefab, _gridRoot.position + _gridRoot.right * x + _gridRoot.forward * y,
                                _gridRoot.rotation, _gridRoot));
                        }
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Destroy grid"))
                {
                    foreach (GameObject gridTile in _grid)
                    {
                        DestroyImmediate(gridTile);
                    }
                    _grid.Clear();
                }
            }
        }
        else
        {
            _selectedTilemap3d = null;
        }

        if (_selectedTilemap3d != null)
        {
            _selectedToolbarIndex = GUILayout.Toolbar(_selectedToolbarIndex, _tools);
        }
        else
        {
            EditorGUILayout.HelpBox("Change Tilemap3D", MessageType.Info);
        }
        
    }
}
