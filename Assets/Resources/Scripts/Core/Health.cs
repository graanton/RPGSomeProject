using System;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public abstract event Action DeathEvent;
    public abstract event Action<int> HitEvent;

    public abstract int CurrentHealth { get; }
    public abstract int MaxHealth { get; }

    public abstract void TakeDamage(int damage);

    public abstract void Heal(int amount);
}