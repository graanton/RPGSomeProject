using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using System;

public class Enemy : Health
{
    [SerializeField] private UnityEvent _deadEvent = new();
    [SerializeField] private int _health, _maxHealth;
    [SerializeField] private DamageEvent _damageEvent = new();

    public HealthEvent playerEvent = new();

    public override UnityEvent DeathEvent => _deadEvent;
    public override DamageEvent HitEvent => _damageEvent;
    public override int CurrentHealth => _health;
    public override int MaxHealth => _maxHealth;

    private void Awake()
    {
        DeathEvent.AddListener(() => DestroyServerRpc());
    }

    public void SetTargetDetecter(IncomingAndOutgoingWatcher detecter)
    {
        detecter.enterEvent.AddListener(
            (Health player) => playerEvent?.Invoke(player));
    }

    public override void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            Debug.LogError("Invalid damage value");
            return;
        }
        if (IsOwner)
        {
            _health -= damage;
            _damageEvent?.Invoke(damage);
            if (_health <= 0)
            {
                _deadEvent?.Invoke();
            }
        }
    }

    [ServerRpc]
    private void DestroyServerRpc()
    {
        Destroy(gameObject);
    }

    public override void Heal(int amount)
    {
        throw new NotImplementedException();
    }
}
