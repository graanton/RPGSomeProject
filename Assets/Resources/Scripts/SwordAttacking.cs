using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

public class SwordAttack : NetworkBehaviour
{
    [SerializeField] private SwordData _data;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _attackMask;
    [SerializeField] private float _attackDelay = 0.5f;
    [SerializeField] private bool _autoAttack = true;

    public UnityEvent AttackEvent = new();

    private float _lastAttackTime = 0f;

    private void Update()
    {
        if (_autoAttack)
        {
            bool haveTargets = Physics.OverlapSphereNonAlloc(_attackPoint.position,
            _data.Length + _data.Width, new Collider[1], _attackMask) > 0;
            if (haveTargets)
            {
                AttackInFront();
            }
        }
    }

    public void AttackInFront()
    {
        if (!IsOwner)
        {
            return;
        }

        if (Time.time - _lastAttackTime < _attackDelay)
        {
            return;
        }

        AttackEvent?.Invoke();

        var targets = Physics.OverlapSphere(_attackPoint.position,
            _data.Length + _data.Width, _attackMask);

        foreach (Collider hit in targets)
        {
            if (hit.TryGetComponent(out Health damageableComponent))
            {
                damageableComponent.TakeDamage(_data.Damage);
            }
        }

        _lastAttackTime = Time.time;
    }
}
