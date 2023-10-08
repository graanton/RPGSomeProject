using System;
using Resources.Scripts.Common;
using UnityEngine;

public abstract class Room : MonoBehaviour
{
    [field: SerializeField] public Vector2Int Size { get; private set; }

    public event Action<Connect> ConnectedEvent;

    public void SetConnectionData(Connect connect)
    {
        ConnectedEvent?.Invoke(connect);
    }

    public virtual void Initialize() { }
}
