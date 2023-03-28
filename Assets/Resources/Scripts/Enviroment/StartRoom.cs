using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : LockedRoomBase
{
    public override bool IsLocked()
    {
        return false;
    }
}
