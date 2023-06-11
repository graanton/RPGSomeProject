using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class LinearShooting : NetworkBehaviour, IAttack
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _fireRate = 1;
    [SerializeField] private NetworkObject _owner;

    private float _lastShootTime;
    private bool _isShooting;
    private IEnumerator _shooting;

    public void StartAttacking()
    {
        if (_isShooting == false)
        {
            _shooting = Shooting();
            StartCoroutine(_shooting);
            _isShooting = true;
        }
    }

    public void StopAttacking()
    {
        if (_isShooting)
        {
            StopCoroutine(_shooting);
            _isShooting = false;
        }
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

    private IEnumerator Shooting()
    {
        while (true)
        {
            Shoot();
            yield return null;
        }
    }
}
