using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Gun : Weapon
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private LineRenderer _lineRenderer;

    private Health _owner;
    private bool _isWantingAttack;

    public UnityEvent WantAttackEvent = new();
    public UnityEvent AttackEvent = new();

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
            ShootServerRpc(_owner.OwnerClientId);
        }
    }

    public override void SetOwner(Health owner)
    {
        _owner = owner;
    }

    [ServerRpc]
    private void ShootServerRpc(ulong ownerClientId)
    {
        Projectile createdProjectile = Instantiate(_projectilePrefab,
            _muzzle.transform.position, _muzzle.transform.rotation);
        createdProjectile.SetOwnerNetworkObjectId(ownerClientId);
    }
}