using UnityEngine;
using UnityEngine.Events;

public abstract class LockedRoomBase: Room
{
    public UnityEvent OpenEvent = new();
    public UnityEvent LockEvent = new();

    public abstract bool IsLocked();
    public void Lock()
    {
        LockEvent?.Invoke();
    }
    public void Open()
    {
        OpenEvent?.Invoke();
    }
}
