using System;
using UnityEngine;
using UnityEngine.Events;

public class RoomMover : MonoBehaviour
{
    public event Action<Room> enterEvent;
    public event Action<Room> exitEvent;

    private void OnTriggerEnter(Collider other)
    {
        TryInvokeEvent(other, enterEvent);
    }
    
    private void OnTriggerExit(Collider other)
    {
        TryInvokeEvent(other, exitEvent);
    }

    private void TryInvokeEvent(Collider collider, Action<Room> roomEvent)
    {
        if (collider.TryGetComponent(out IncomingAndOutgoingWatcher watcher))
        {
            if (watcher.TryGetComponent(out Room room))
            {
                roomEvent?.Invoke(room);
            }
            else
            {
                throw new System.Exception("Add Room component for watcher");
            }
        }
    }
}