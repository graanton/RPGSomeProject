using System.Collections.Generic;
using UnityEngine;

namespace CustomTilemap3d
{
    public class Tilemap3D : MonoBehaviour
    {
        [SerializeField] private Vector2Int _size;
        [SerializeField] private Transform _tilesRoot;

        public Vector2Int size => _size;

        private Vector3 _gridOffset => new Vector3(0.5f, 0, 0.5f);
        private List<GameObject> _grid = new();

        public void ResetGridList()
        {
            _grid = new List<GameObject>(new GameObject[_size.x * _size.y]);
        }

        public void SetTile(GameObject tile, Vector2Int position)
        {
            if (position.x > _size.x || position.y > _size.y || position.x < 0 || position.y < 0 ||
                _grid[GetGridIndex(position)] != null) return;

            Vector3 instantiatePosition = _tilesRoot.position + _tilesRoot.right * (_gridOffset.x + position.x) + _tilesRoot.forward * (_gridOffset.z + position.y);
            _grid[GetGridIndex(position)] = Instantiate(tile, instantiatePosition, _tilesRoot.rotation, _tilesRoot);
        }

        public void ClearCell(Vector2Int position)
        {
            DestroyImmediate(_grid[GetGridIndex(position)]);
            _grid[GetGridIndex(position)] = null;
        }

        private int GetGridIndex(Vector2Int position)
        {
            return _size.y * position.x + position.y;
        }
    }
}