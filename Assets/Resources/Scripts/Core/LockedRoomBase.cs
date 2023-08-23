using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class LockedRoomBase: Room
{
    public abstract event Action OpenEvent;
    public abstract event Action LockEvent;

    public abstract bool IsLocked();

    public virtual bool TryLock() => false;
    public virtual bool TryOpen()=> false;
}
