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
            AttackInFront();
        }
    }

    public void AttackInFront()
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
