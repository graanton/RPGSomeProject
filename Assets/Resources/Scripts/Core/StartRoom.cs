using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StartRoom : LockedRoomBase
{
    private const bool IS_LOCKED = false;

    public override bool IsLocked()
    {
        return IS_LOCKED;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Open();
        }
    }
}
