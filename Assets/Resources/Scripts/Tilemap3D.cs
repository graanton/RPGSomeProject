using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tilemap3D : MonoBehaviour
{
    [SerializeField] private Vector2Int _size;

    public Vector2Int size => _size;

    public void SetTile(GameObject tile, Vector2Int position)
    {

    }
}
