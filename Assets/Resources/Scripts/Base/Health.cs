using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class Health : NetworkBehaviour
{
    public abstract UnityEvent DeathEvent { get; }
    public abstract DamageEvent HitEvent { get; }

    public abstract int CurrentHealth { get; }
    public abstract int MaxHealth { get; }

    public abstract void TakeDamage(int damage);

    public abstract void Heal(int amount);
}

public class DamageEvent: UnityEvent<int> { }