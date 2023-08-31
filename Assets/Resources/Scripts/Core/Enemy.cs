using UnityEngine;
using Unity.Netcode;
using System;

public class Enemy : Health
{
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;

    public HealthEvent playerEvent = new();

    public override event Action DeathEvent;
    public override event Action<int> HitEvent;

    public override int CurrentHealth => _health;
    public override int MaxHealth => _maxHealth;

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
        _health -= damage;
        HitEvent?.Invoke(damage);
        if (_health <= 0)
        {
            DeathEvent?.Invoke();
        }
    }

    public override void Heal(int amount)
    {
        throw new NotImplementedException();
    }
}
