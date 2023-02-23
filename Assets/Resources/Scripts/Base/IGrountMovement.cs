using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public interface IGrountMovement
{
    public MovementEvent MoveEvent { get; }

    public void Move(Vector2 direction);
    
}

[Serializable]
public class MovementEvent: UnityEvent<Vector2> { }