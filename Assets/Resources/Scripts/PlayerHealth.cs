using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    [SerializeField, Min(0)] private int _health, _maxHealth;
    [SerializeField] private UnityEvent _deadEvent = new();
    [SerializeField] private DamageEvent _hitEvent = new();
    [SerializeField] private bool _isDead;

    public override int MaxHealth => _maxHealth;
    public override int CurrentHealth => _health;

    public override UnityEvent OnDeath => _deadEvent;
    public override DamageEvent OnHit => _hitEvent;

    public override void OnNetworkSpawn()
    {
        _deadEvent.AddListener(DestroyServerRpc);
    }

    [ServerRpc]
    private void DestroyServerRpc()
    {
        Destroy(gameObject);
    }

    public override void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            Debug.LogError("Invalid damage value");
            return;
        }

        if (IsOwner)
        {
            _health -= damage;
            _hitEvent?.Invoke(damage);
            if (_health <= 0)
            {
                _deadEvent?.Invoke();
            }
        }
    }

    public override void Heal(int amount)
    {
        throw new NotImplementedException();
    }
}
