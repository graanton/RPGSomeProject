using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class Room : MonoBehaviour
{
    [SerializeField] private Tilemap _groundTilemap;

    [SerializeField] private RectInt _bounds = new RectInt();

    public RectInt bounds => _bounds;

    public static bool Intersect(RectInt a, RectInt b)
    {
        return !((a.position.x >= (b.position.x + b.size.x)) || ((a.position.x + a.size.x) <= b.position.x)
            || (a.position.y >= (b.position.y + b.size.y)) || ((a.position.y + a.size.y) <= b.position.y));
    }

    public void TilesRectToLocalZeroPosition()
    {
        BoundsInit();
        Vector3 offset = _groundTilemap.transform.right * _bounds.position.x +
            _groundTilemap.transform.forward * _bounds.position.y;

        foreach (Transform tile in _groundTilemap.transform)
        {
            tile.localPosition -= offset;
        }
        BoundsInit();
    }

    public void BoundsInit()
    {
        if (_groundTilemap.transform.childCount == 0)
        {
            throw new Exception("Ground is void");
        }

        var firstTilePosition = TransformToCellPosition2D(_groundTilemap.transform.GetChild(0));

        int mostUpPosition = firstTilePosition.y;
        int mostRightPosition = firstTilePosition.x;
        int mostDownPosition = firstTilePosition.y;
        int mostLeftPosition = firstTilePosition.x;

        for (int i = 0; i < _groundTilemap.transform.childCount; i++)
        {
            Transform tile = _groundTilemap.transform.GetChild(i);
            Vector2Int currentTilePosition = TransformToCellPosition2D(tile);

            if (currentTilePosition.y > mostUpPosition)
            {
                mostUpPosition = currentTilePosition.y;
            }
            if (currentTilePosition.x > mostRightPosition)
            {
                mostRightPosition = currentTilePosition.x;
            }
            if (currentTilePosition.y < mostDownPosition)
            {
                mostDownPosition = currentTilePosition.y;
            }
            if (currentTilePosition.x < mostLeftPosition)
            {
                mostLeftPosition = currentTilePosition.x;
            }

        }
        
        Vector2Int localPosition = new Vector2Int(mostLeftPosition, mostDownPosition);
        Vector2Int position = (Vector2Int)_groundTilemap.LocalToCell(transform.localPosition);
        Vector2Int size = new Vector2Int(mostRightPosition, mostUpPosition) - localPosition;

        size += Vector2Int.one;

        transform.position = AxisConverter.XYToXZInt(position);
        _bounds = new RectInt(localPosition + position, size);
    }

    public void MoveBoundsPosition(Vector2Int newPosition)
    {
        _bounds = new RectInt(newPosition, bounds.size);
    }

    private Vector2Int TransformToCellPosition2D(Transform target)
    {
        return (Vector2Int)_groundTilemap.WorldToCell(target.position);
    }
}
