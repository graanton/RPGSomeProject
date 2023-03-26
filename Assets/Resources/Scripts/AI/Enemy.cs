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

    public PlayerEvent playerEvent = new();

    public override UnityEvent OnDeath => _deadEvent;
    public override DamageEvent OnHit => _damageEvent;
    public override int CurrentHealth => _health;
    public override int MaxHealth => _maxHealth;

    private void Awake()
    {
        OnDeath.AddListener(() => Destroy(gameObject));
    }

    public void SetTargetDetecter(IncomingAndOutgoingWatcher detecter)
    {
        detecter.enterEvent.AddListener(TargetInvoke);
    }

    private void TargetInvoke(PlayerHealth player) => playerEvent?.Invoke(player);

    public override void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            Debug.LogError("Invalid damage value");
            return;
        }
        if (IsServer)
        {
            TakeDamageClientRpc(damage);
        }
    }

    [ClientRpc]
    private void TakeDamageClientRpc(int damage)
    {
        
    }

    public override void Heal(int amount)
    {
        throw new System.NotImplementedException();
    }
}
