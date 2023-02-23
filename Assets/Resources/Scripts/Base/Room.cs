using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Unity.Netcode;
using System.Collections.Generic;
using UnityEngine.Events;

public class Room : NetworkBehaviour, IBoundsCorrectble
{
    [SerializeField] private Tilemap _groundTilemap;
    [SerializeField] private RectInt _bounds = new RectInt();

    public RoomEvent neighboreAddEvent = new();

    public RectInt localBounds => _bounds;

    private Dictionary<Vector2Int, Hallway> _buffer = new();

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

        _bounds = new RectInt(localPosition + position, size);
    }

    public void CorrectPosition()
    {
        transform.position = AxisConverter.XYToXZInt(_bounds.position);
    }

    public void MoveBoundsPosition(Vector2Int newPosition)
    {
        _bounds = new RectInt(newPosition, localBounds.size);
    }

    private Vector2Int TransformToCellPosition2D(Transform target)
    {
        return (Vector2Int)_groundTilemap.WorldToCell(target.position);
    }

    public void AddBufferedHallway(Hallway hallway)
    {
        _buffer[hallway.localBounds.position] = hallway;
        neighboreAddEvent?.Invoke(hallway);
    }

    public bool OnTheBuffer(RectInt other)
    {
        return localBounds.Overlaps(other) == false &&
            new RectInt(localBounds.position - Vector2Int.one,
            localBounds.size + Vector2Int.one * 2).Overlaps(other);
    }
}

public class RoomEvent: UnityEvent<Room> { }
public interface IBoundsCorrectble
{
    public RectInt localBounds { get; }
    public void MoveBoundsPosition(Vector2Int newPosition);
    public void CorrectPosition();
    public void BoundsInit();
    public void TilesRectToLocalZeroPosition();
}