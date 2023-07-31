using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class Health : NetworkBehaviour
{
    public abstract event Action DeathEvent;
    public abstract event Action<int> HitEvent;

    public abstract int CurrentHealth { get; }
    public abstract int MaxHealth { get; }

    public abstract void TakeDamage(int damage);

    public abstract void Heal(int amount);
}