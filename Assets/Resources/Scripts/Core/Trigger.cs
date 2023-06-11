using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public ColliderEvent OnTriggerEnterEvent;
    public ColliderEvent OnTriggerExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }
}

[Serializable]
public class ColliderEvent: UnityEvent<Collider> { }