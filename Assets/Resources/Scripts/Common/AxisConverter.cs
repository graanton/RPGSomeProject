using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AxisConverter
{
    public static Vector3Int XYToXZInt(Vector2Int xyVector)
    {
        return new Vector3Int(xyVector.x, 0, xyVector.y);
    }

    public static Vector3 XYToXZ(Vector2 xyVector)
    {
        return new Vector3(xyVector.x, 0, xyVector.y);
    }

    public static Vector3 XYZToXZ(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }
}
