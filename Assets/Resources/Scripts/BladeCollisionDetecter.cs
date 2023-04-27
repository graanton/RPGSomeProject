using System;
using UnityEngine;
using UnityEngine.Events;

public class BladeCollisionDetecter : MonoBehaviour
{
    public CollisionEvent CutEvent = new();

    private void OnCollisionEnter(Collision collision)
    {
        CutEvent?.Invoke(collision);
    }
}

[Serializable]
public class CollisionEvent: UnityEvent<Collision> { }