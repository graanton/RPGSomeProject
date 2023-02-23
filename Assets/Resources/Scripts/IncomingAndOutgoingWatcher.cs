using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IncomingAndOutgoingWatcher : MonoBehaviour
{
    public PlayerEvent enterEvent = new();
    public PlayerEvent leaveEvent = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth player))
        {
            enterEvent?.Invoke(player);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth player))
        {
            leaveEvent?.Invoke(player);
        }
    }
}

[Serializable]
public class PlayerEvent: UnityEvent<PlayerHealth> { }