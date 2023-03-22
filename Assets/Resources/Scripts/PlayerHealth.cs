using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : NetworkBehaviour, IDamageable
{
    [SerializeField, Min(0)] private int _health, _maxHealth;
    [SerializeField] private UnityEvent _deadEvent = new();
    
    public int Health => _health;
    public int MaxHealth => _maxHealth;

    public UnityEvent DeadEvent => _deadEvent;
    public UnityEvent HitEvent = new();

    private NetworkObject _networkObject;

    private void Awake()
    {
        _networkObject = GetComponent<NetworkObject>();
    }

    public override void OnNetworkSpawn()
    {
        DeadEvent.AddListener(OnDead);
    }

    private void OnDead()
    {
        if (_networkObject.IsSpawned)
        {
            _networkObject.Despawn();
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            Debug.LogError("Is not damage");
            return;
        }

        if (IsServer)
        {
            HitClientRpc(damage);
        }
    }

    [ClientRpc]
    private void HitClientRpc(int damage)
    {
        _health -= damage;
        HitEvent?.Invoke();
        if (_health <= 0)
        {
            _deadEvent?.Invoke();
        }
    }
}
