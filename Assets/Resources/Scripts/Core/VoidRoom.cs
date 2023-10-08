using System;
using UnityEngine;

public class VoidRoom : LockedRoomBase
{
    public override event Action OpenEvent;
    public override event Action LockEvent;
    public override bool IsLocked()
    {
        return false;
    }

    public override void Lock()
    {
        throw new Exception("Room can not be lock");
    }

    public override void Open()
    {
        throw new Exception("Room can not be open");
    }
}
