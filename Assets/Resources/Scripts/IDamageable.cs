using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    public int Health { get; }
    public int MaxHealth { get; }
    public UnityEvent DeadEvent { get; }

    public void TakeDamage(int damage);
}
