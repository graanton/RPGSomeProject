using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class Room : MonoBehaviour
{
    [SerializeField] private Transform _tilesRoot;

    public RectInt bounds { get; private set; }

    public static bool Intersect(RectInt a, RectInt b)
    {
        return !((a.position.x >= (b.position.x + b.size.x)) || ((a.position.x + a.size.x) <= b.position.x)
            || (a.position.y >= (b.position.y + b.size.y)) || ((a.position.y + a.size.y) <= b.position.y));
    }

    public void BoundsInit()
    {
        Transform mostTop, mostRight, mostBottom, mostLeft;

        mostTop = _tilesRoot;
        mostRight = _tilesRoot;
        mostBottom = _tilesRoot;
        mostLeft = _tilesRoot;

        foreach (Transform tile in _tilesRoot.transform)
        {
            if (tile.position.z > mostTop.position.z)
            {
                mostTop = tile;
            }
            if (tile.position.z < mostBottom.position.z)
            {
                mostBottom = tile;
            }
            if (tile.position.x > mostRight.position.x)
            {
                mostRight = tile;
            }
            if (tile.position.x < mostLeft.position.x)
            {
                mostLeft = tile;
            }
        }
        Vector2Int localPosition = new (Mathf.RoundToInt(mostLeft.localPosition.x), Mathf.RoundToInt(mostBottom.localPosition.x));
        Vector2Int size = new (Mathf.RoundToInt(mostRight.localPosition.x + localPosition.x), Mathf.RoundToInt(mostTop.localPosition.z + localPosition.y));

        bounds = new RectInt(Vector2Int.zero, size);
    }

    public void MoveBounds(Vector2Int newPosition)
    {
        bounds = new RectInt(newPosition, bounds.size);
    }
}
        