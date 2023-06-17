using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Weapon : NetworkBehaviour, IAttack
{
    public abstract void StartAttacking();
    public abstract void StopAttacking();
    public abstract void SetOwner(Health owner);
}
