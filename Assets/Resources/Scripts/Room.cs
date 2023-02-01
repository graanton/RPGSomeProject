using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class Room : MonoBehaviour
{
    private RectInt _bounds = new RectInt();

    public RectInt bounds => _bounds;

    private void OnValidate()
    {
        BoundsSizeInit();
    }

    public static bool Intersect(RectInt a, RectInt b)
    {
        return !((a.position.x >= (b.position.x + b.size.x)) || ((a.position.x + a.size.x) <= b.position.x)
            || (a.position.y >= (b.position.y + b.size.y)) || ((a.position.y + a.size.y) <= b.position.y));
    }

    public void BoundsSizeInit()
    {
        throw new System.Exception("FIIIIIX IT");
    }

    public void MoveBoundsPosition(Vector2Int newPosition)
    {
        _bounds = new RectInt(newPosition, bounds.size);
    }
}
        