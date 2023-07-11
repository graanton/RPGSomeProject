using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract WeaponConfig Config { get; }

    public abstract void StartAttacking();
    public abstract void StopAttacking();
    public abstract void SetOwner(Health owner);
}
