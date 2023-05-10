using Unity.Netcode;
using UnityEngine;

public class LinearShooting : NetworkBehaviour, IAttack
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _fireRate = 1;
    [SerializeField] private NetworkObject _owner;

    private float _lastShootTime;

    public void StartAttacking()
    {
        Shoot();
    }

    public void StopAttacking()
    {
        throw new System.NotImplementedException();
    }

    [ServerRpc]
    private void ShootServerRpc(ulong clientId, ulong ownerId)
    {
        var newProjectile = Instantiate(_projectilePrefab,
            _attackPoint.position, _attackPoint.rotation);
        newProjectile.SetOwnerNetworkObjectId(ownerId);

        var networkedProjectile = newProjectile.GetComponent<NetworkObject>();
        networkedProjectile.SpawnAsPlayerObject(clientId, true);

        _lastShootTime = Time.time;
    }

    private void Shoot()
    {
        if (Time.time - _lastShootTime > _fireRate)
        {
            ShootServerRpc(OwnerClientId, _owner.NetworkObjectId);
        }
    }
}
