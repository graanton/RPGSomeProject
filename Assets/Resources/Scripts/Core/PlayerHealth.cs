using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    [SerializeField, Min(0)] private int _health, _maxHealth;
    [SerializeField] private bool _isDead;

    public override int MaxHealth => _maxHealth;
    public override int CurrentHealth => _health;


    

    private float _invulnerableTime;

    public override event Action DeathEvent;
    public override event Action<int> HitEvent;
    public event Action<int> InvulnerableHitEvent;

    public override void OnNetworkSpawn()
    {
        DeathEvent += DestroyServerRpc;
    }

    public void BecomeInvulnerable(float time)
    {
        if (time <= 0)
        {
            Debug.LogError("Invalid time value");
            return;
        }
        if (_invulnerableTime < time)
        {
            _invulnerableTime = time;
        }
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
            if (_invulnerableTime <= 0)
            {
                _health -= damage;
                HitEvent?.Invoke(damage);
                if (_health <= 0)
                {
                    DeathEvent?.Invoke();
                }
            }
            else
            {
                InvulnerableHitEvent?.Invoke(damage);
            }
        }
    }

    public override void Heal(int amount)
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        if (_invulnerableTime > 0)
        {
            _invulnerableTime -= Time.deltaTime;
        }
    }

    [ServerRpc]
    private void DestroyServerRpc()
    {
        Destroy(gameObject);
    }
}
