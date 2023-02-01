using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

namespace
{
    public class Tilemap3DEditorWindow : EditorWindow
    {
        [SerializeField] private GameObject _gridTilePrefab;
        private const string Title = "Tilemap 3D";

        private int _selectedToolbarIndex = -1;
        private int _selectedTilemap3dIndex = -1;
        private Tilemap3D _selectedTilemap3d;
        private string[] _tools = new string[3] { "Brush", "Eraser", "None" };
        private List<GameObject> _grid = new();
        private Transform _gridRoot;
        private GameObject _currentTileForBrush;
        private Vector3 _gridOffset => new Vector3(0.5f, 0, 0.5f);

        private UnityEvent _tilemap3dChangeEvent = new();

        [MenuItem("Window/Tilemap 3D")]
        private static void ShowWindow()
        {
            var window = GetWindow<Tilemap3DEditorWindow>();
            window.titleContent.text = Title;
            window.Show();
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            _tilemap3dChangeEvent.AddListener(OnTilemap3dChanged);
        }

        private void OnTilemap3dChanged()
        {
            _grid.Clear();
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            _tilemap3dChangeEvent.RemoveListener(OnTilemap3dChanged);
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            switch (_selectedToolbarIndex)
            {
                case 1:
                case 0:
                    Tools.current = Tool.None;
                    if (Event.current.type == EventType.MouseDown && _selectedTilemap3d)
                    {
                        Camera camera = sceneView.camera;
                        Vector2 mousPosition = new Vector2(Event.current.mousePosition.x, sceneView.position.size.y - Event.current.mousePosition.y);
                        Ray ray = camera.ScreenPointToRay(mousPosition);

                        if (_selectedTilemap3d.gameObject.scene.GetPhysicsScene().Raycast(ray.origin, ray.direction, out RaycastHit hit))
                        {
                            GameObject gridTile = hit.collider.gameObject;
                            Vector2Int tilePosition = FindGridSlotPosition(gridTile);

                            if (_selectedToolbarIndex == 0)
                            {
                                _selectedTilemap3d.SetTile(_currentTileForBrush, tilePosition);
                            }
                            else
                            {
                                _selectedTilemap3d.ClearCell(tilePosition);
                            }
                            EditorUtility.SetDirty(_selectedTilemap3d);
                        }
                    }
                    break;
                case 2:
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

            bool canChangeTilemap3d = tileMaps3d.Count > 0 && _selectedTilemap3dIndex < tileMaps3d.Count + 1;
            if (canChangeTilemap3d)
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

            bool tilemap3dIsSelected = _selectedTilemap3dIndex != 0 &&
                _selectedTilemap3dIndex >= 1 && tileMaps3d[_selectedTilemap3dIndex - 1];

            if (tilemap3dIsSelected)
            {
                if (tileMaps3d[_selectedTilemap3dIndex - 1] != _selectedTilemap3d)
                {
                    _tilemap3dChangeEvent?.Invoke();
                }
                _selectedTilemap3d = tileMaps3d[_selectedTilemap3dIndex - 1];

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Grid tile prefab");
                _gridTilePrefab = (GameObject)EditorGUILayout.ObjectField(_gridTilePrefab, typeof(GameObject));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Grid root");
                _gridRoot = (Transform)EditorGUILayout.ObjectField(_gridRoot, typeof(Transform));
                GUILayout.EndHorizontal();

                GridButton();

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Current Tile");
                _currentTileForBrush = (GameObject)EditorGUILayout.ObjectField(_currentTileForBrush, typeof(GameObject));
                GUILayout.EndHorizontal();


            }
            else
            {
                _selectedTilemap3d = null;
            }

            if (tilemap3dIsSelected)
            {
                _selectedToolbarIndex = GUILayout.Toolbar(_selectedToolbarIndex, _tools);
            }
            else
            {
                EditorGUILayout.HelpBox("Change Tilemap3D", MessageType.Info);
            }
        }

        private void GridButton()
        {
            if (_grid.Count == 0)
            {
                if (GUILayout.Button("Create grid"))
                {
                    for (int x = 0; x < _selectedTilemap3d.size.x; x++)
                    {
                        for (int y = 0; y < _selectedTilemap3d.size.y; y++)
                        {
                            Vector3 gridTilePostion = _gridRoot.position + _gridRoot.right * (_gridOffset.x + x)
                                + _gridRoot.forward * (_gridOffset.z + y);

                            _grid.Add(Instantiate(_gridTilePrefab, gridTilePostion,
                                _gridRoot.rotation, _gridRoot));

                            _grid[x * _selectedTilemap3d.size.y + y].name += $" ({x}, {y})";
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

        private Vector2Int FindGridSlotPosition(GameObject gridSlot)
        {
            for (int x = 0; x < _selectedTilemap3d.size.x; x++)
            {
                for (int y = 0; y < _selectedTilemap3d.size.y; y++)
                {
                    GameObject currentGridTile;
                    try
                    {
                        currentGridTile = _grid[x * _selectedTilemap3d.size.y + y];
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new Exception("Mayby you miss root?");
                    }
                    if (currentGridTile == gridSlot)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }
            throw new Exception("Not find grid tile");
        }
    }
}