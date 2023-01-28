using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class Room : MonoBehaviour
{
    [SerializeField] private Tilemap3D _tilemap;

    private RectInt _bounds = new RectInt();

    public RectInt bounds => _bounds;

    private void Awake()
    {
        BoundsSizeInit();
    }

    public static bool Intersect(RectInt a, RectInt b)
    {
        return !((a.position.x >= (b.position.x + b.size.x)) || ((a.position.x + a.size.x) <= b.position.x)
            || (a.position.y >= (b.position.y + b.size.y)) || ((a.position.y + a.size.y) <= b.position.y));
    }

    private void BoundsSizeInit()
    {
        _bounds = new RectInt(_bounds.position, _tilemap.size);
    }

    public void MoveBoundsPosition(Vector2Int newPosition)
    {
        _bounds = new RectInt(newPosition, bounds.size);
    }
}
        