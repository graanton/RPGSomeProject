using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Tilemap3DEditor : EditorWindow
{
    [SerializeField] private GameObject _gridTilePrefab;

    private int _selectedToolbarIndex = -1;
    private int _selectedTilemap3dIndex = -1;
    private Tilemap3D _selectedTilemap3d;
    private string[] _tools = new string[2] { "Brush", "None" };
    private List<GameObject> _grid = new();
    private Transform _gridRoot;

    private Vector3 _gridOffset => new Vector3(0.5f, 0, 0.5f);

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
                Tools.current = Tool.None;
                if (_selectedTilemap3d)
                {
                    Camera camera = ((SceneView)SceneView.sceneViews[0]).camera;
                    Ray ray = camera.ScreenPointToRay(Event.current.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        for (int x = 0; x < _selectedTilemap3d.size.x; x++)
                        {
                            for (int y = 0; y < _selectedTilemap3d.size.y; y++)
                            {
                                var currentGridTile = _grid[_selectedTilemap3d.size.y * x + y];
                                if (currentGridTile == hit.collider.gameObject)
                                {
                                    _selectedTilemap3d.SetTile(currentGridTile, new Vector2Int(x, y));
                                    break;
                                }
                            }
                        }
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
        var tileMaps3d = new List<Tilemap3D>(FindObjectsOfType<Tilemap3D>());
        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null && prefabStage.prefabContentsRoot.TryGetComponent(out Tilemap3D prefabTilemap3d))
        {
            tileMaps3d.Add(prefabTilemap3d);
        }
        var tileMaps3dNames = new string[tileMaps3d.Count + 1];
        tileMaps3dNames[0] = "None";

        for (int i = 1; i < tileMaps3d.Count + 1; i++) 
        {
            tileMaps3dNames[i] = tileMaps3d[i - 1].name;
        }

        if (tileMaps3d.Count > 0 && _selectedTilemap3dIndex < tileMaps3d.Count + 1)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Selected Tilemap3D");
            _selectedTilemap3dIndex = EditorGUILayout.Popup(_selectedTilemap3dIndex, tileMaps3dNames);

            GUILayout.EndHorizontal();
        }
        else
        {
            _selectedTilemap3dIndex = -1;
        }
        
        if (_selectedTilemap3dIndex >= 1 && tileMaps3d[_selectedTilemap3dIndex - 1])
        {
            _selectedTilemap3d = tileMaps3d[_selectedTilemap3dIndex - 1];
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
                            _grid.Add(Instantiate(_gridTilePrefab, _gridRoot.position + _gridRoot.right * (_gridOffset.x + x) + _gridRoot.forward * (_gridOffset.z + y),
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

    private Vector2Int GetGridSlotPosition(GameObject gridSlot)
    {
        return Vector2Int.zero;
    }
}
