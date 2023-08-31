using UnityEngine;
using UnityEngine.Events;

public class Gun : Weapon
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _muzzle;

    private Health _owner;
    private bool _isWantingAttack;

    public UnityEvent WantAttackEvent = new();
    public UnityEvent AttackEvent = new();

    public override WeaponConfig Config => throw new System.NotImplementedException();

    public override void StartAttacking()
    {
        if (!_isWantingAttack)
        {
            WantAttackEvent?.Invoke();
            _isWantingAttack = true;
        }
    }

    public override void StopAttacking()
    {
        if (_isWantingAttack)
        {
            AttackEvent?.Invoke();
            _isWantingAttack = false;
        }
    }

    public override void SetOwner(Health owner)
    {
        _owner = owner;
    }
}