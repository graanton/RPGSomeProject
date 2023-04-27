using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class LockedRoomBase: Room
{
    public UnityEvent OpenEvent = new();
    public UnityEvent LockEvent = new();

    public abstract bool IsLocked();
    public virtual void Lock()
    {
        CloseClientRpc();
    }

    public virtual void Open()
    {
        OpenClientRpc();
    }

    [ClientRpc]
    private void OpenClientRpc()
    {
        OpenEvent?.Invoke();
    }

    [ClientRpc]
    private void CloseClientRpc()
    {
        LockEvent?.Invoke();
    }
}
