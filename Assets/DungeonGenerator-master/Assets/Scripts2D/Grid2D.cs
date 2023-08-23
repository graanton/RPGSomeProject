﻿using UnityEngine;

public class Grid2D<T>
{
    T[] data;

    public Vector2Int Size { get; private set; }
    public Grid2D(Vector2Int size)
    {
        Size = size;

        data = new T[size.x * size.y];
    }

    public int GetIndex(Vector2Int pos)
    {
        return pos.x + (Size.x * pos.y);
    }

    public bool InBounds(Vector2Int pos)
    {
        return new RectInt(Vector2Int.zero, Size).Contains(pos);
    }

    public T this[int x, int y]
    {
        get
        {
            return this[new Vector2Int(x, y)];
        }
        set
        {
            this[new Vector2Int(x, y)] = value;
        }
    }

    public T this[Vector2Int pos]
    {
        get
        {
            return data[GetIndex(pos)];
        }
        set
        {
            data[GetIndex(pos)] = value;
        }
    }
}
