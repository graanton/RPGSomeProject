using UnityEngine;
using UnityEngine.Events;

public class RoomMover : MonoBehaviour
{
    public RoomEvent enterEvent = new();
    public RoomEvent exitEvent = new();

    private void OnTriggerEnter(Collider other)
    {
        TryInvokeEvent(other, enterEvent);
    }
    
    private void OnTriggerExit(Collider other)
    {
        TryInvokeEvent(other, exitEvent);
    }

    private void TryInvokeEvent(Collider collider, RoomEvent roomEvent)
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