using System;
using Resources.Scripts.Common;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Vector2Int _size;

    public Vector2Int Size => _size;

    public event Action<Connect> ConnectedEvent;

    public void SetConnectionData(Connect connect)
    {
        ConnectedEvent?.Invoke(connect);
    }
}
