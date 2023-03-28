using System;
using UnityEngine;
using UnityEngine.Events;

public class IncomingAndOutgoingWatcher : MonoBehaviour
{
    public HealthEvent enterEvent = new();
    public HealthEvent leaveEvent = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health player))
        {
            enterEvent?.Invoke(player);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Health player))
        {
            leaveEvent?.Invoke(player);
        }
    }
}

[Serializable]
public class HealthEvent: UnityEvent<Health> { }