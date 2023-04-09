using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using System;
using Unity.Netcode.Components;

[RequireComponent(typeof(Rigidbody), typeof(NetworkRigidbody))]
public class Projectile : NetworkBehaviour
{
    [SerializeField] private float _maxLifeTime;
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _targetsLayer;
    [SerializeField] private LayerMask _obstaclesLayer;
    [SerializeField] private int _damage;
    
    public HitEvent hitEvent = new();

    private Vector3 _direction;
    private NetworkObject _owner;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetOwnerNetworkObjectId(ulong id)
    {
        _owner = GetNetworkObject(id);
    }

    private void Update()
    {
        if (IsOwner)
        {
            _rigidbody.velocity = _direction.normalized * _speed;
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Destroy(gameObject, _maxLifeTime);
        }
        if (IsOwner)
        {
            _direction = transform.forward;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out NetworkObject target) &&
            target != _owner)
        {
            if (IsLayerInMask(other.gameObject.layer, _targetsLayer) &&
                other.TryGetComponent(out Health damageClaimer))
            {
                damageClaimer.TakeDamage(_damage);
                hitEvent?.Invoke(damageClaimer);
            }
            if (IsLayerInMask(other.gameObject.layer, _obstaclesLayer))
            {
                Destroy(gameObject);
            }
        }
    }

    private bool IsLayerInMask(int otherLayer, LayerMask mask)
    {
        return mask == (mask | (1 << otherLayer));
    }
}

[Serializable]
public class HitEvent: UnityEvent<Health> { }