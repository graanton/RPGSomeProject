using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : NetworkBehaviour, IHitble
{
    [SerializeField] private int _health, _maxHealth;

    public int Health => _health;
    public int MaxHealth => _maxHealth;

    public UnityEvent DeadEvent => throw new System.NotImplementedException();

    public void Hit(int damage)
    {
        if (damage <= 0)
        {
            Debug.LogError("Is not damage");
            return;
        }

        _health -= damage;
    }
}
