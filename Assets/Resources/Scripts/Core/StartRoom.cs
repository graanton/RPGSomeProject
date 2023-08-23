using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StartRoom : LockedRoomBase
{
    private bool _isLocked = true;

    public override event Action OpenEvent;
    public override event Action LockEvent;

    public override bool IsLocked() => _isLocked;

    private void Start()
    {
        TryOpen();
    }

    public override bool TryOpen()
    {
        if (!_isLocked)
        {
            return false;
        }
        _isLocked = false;
        OpenEvent?.Invoke();
        return true;
    }
}
