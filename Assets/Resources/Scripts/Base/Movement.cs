using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class Movement: NetworkBehaviour
{
    public abstract MovementEvent MoveEvent { get; }

    public abstract void Move(Vector3 direction);
    
}

[Serializable]
public class MovementEvent: UnityEvent<Vector3> { }