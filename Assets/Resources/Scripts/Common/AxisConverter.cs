using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AxisConverter
{
    public static Vector3 XYToXZ(Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }

    public static Vector3 XYZToXZ(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }
    
    public static Vector2Int XZToXYInt(Vector3 vector)
    {
        return new Vector2Int((int)vector.x, (int)vector.z);
    }
}
