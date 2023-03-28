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

    public Vector3 direction;
    public HitEvent hitEvent = new();

    private GameObject _owner;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetOwner(GameObject owner)
    {
        _owner = owner;
    }

    private void Update()
    {
        if (IsOwner)
        {
            _rigidbody.velocity = direction.normalized * _speed;
        }
    }

    public override void OnNetworkSpawn()
    {
        Destroy(gameObject, _maxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != _owner)
        {
            if (IsLayerInMask(other.gameObject.layer, _obstaclesLayer))
            {
                Destroy(gameObject);
            }
            if (IsLayerInMask(other.gameObject.layer, _targetsLayer) &&
                other.TryGetComponent(out Health damageClaimer))
            {
                damageClaimer.TakeDamage(_damage);
                hitEvent?.Invoke(damageClaimer);
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