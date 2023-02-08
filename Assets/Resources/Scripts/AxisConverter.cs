using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AxisConverter
{
    public static Vector3Int XYToXZInt(Vector2Int xyVector)
    {
        return new Vector3Int(xyVector.x, 0, xyVector.y);
    }
}
