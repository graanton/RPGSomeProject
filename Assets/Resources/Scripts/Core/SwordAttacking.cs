using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

public class SwordAttack : NetworkBehaviour
{
    [SerializeField] private LayerMask _attackMask;
    [SerializeField] private float _attackDelay = 0.5f;
    [SerializeField] private float _attackrange = 1f;
    [SerializeField] private Transform _attackPoint;

    public UnityEvent AttackEvent = new();

    private float _lastAttackTime = 0f;

    private void Update()
    {

    }

    private void AttackInFront()
    {
        if (!IsOwner)
        {
            Debug.LogWarning("Only owner can Attack");
            return;
        }

        if (Time.time - _lastAttackTime < _attackDelay)
        {
            return;
        }

        AttackEvent?.Invoke();

        _lastAttackTime = Time.time;
    }
}
