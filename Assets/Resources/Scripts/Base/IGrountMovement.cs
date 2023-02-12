using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public interface IGrountMovement
{
    public MoveEvent moveEvent { get; }

    public void Move(Vector2 direction);
    
}

[Serializable]
public class MoveEvent: UnityEvent<Vector2> { }