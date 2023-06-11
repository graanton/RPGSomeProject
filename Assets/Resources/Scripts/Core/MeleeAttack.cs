using UnityEngine;
using UnityEngine.Events;

public class MeleeAttack : MonoBehaviour, IAttack
{
    [SerializeField] private SwordData _swordData;
    [SerializeField] private Transform _attackPoint;

    public UnityEvent StartAttackEvent = new();
    public UnityEvent StopAttackEvent = new();

    private bool _isAttacking;

    private void Awake()
    {
        StopAttackEvent.AddListener(Swing);
    }

    public void StartAttacking()
    {
        if (_isAttacking) return;

        StartAttackEvent?.Invoke();
        _isAttacking = true;
    }

    public void StopAttacking()
    {
        if (_isAttacking == false) return;

        StopAttackEvent?.Invoke();
        _isAttacking = false;
    }

    private void Swing()
    {
        
    }
}
