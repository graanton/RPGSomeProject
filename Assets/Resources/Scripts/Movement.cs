using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Movement : MonoBehaviour
{
    public abstract MoveEvent _moveEvent { get; }

    public abstract void Move(Vector2 direction);
}

[Serializable]
public class MoveEvent: UnityEvent<Vector2> { }