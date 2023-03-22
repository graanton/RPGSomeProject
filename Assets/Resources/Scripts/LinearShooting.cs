using Unity.Netcode;
using UnityEngine;

public class LinearShooting : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _fireRate = 1;

    private float _lastShootTime;

    public void Shoot()
    {
        if (Time.time - _lastShootTime > _fireRate)
        {
            var newProjectile = Instantiate(_projectilePrefab,
            _attackPoint.position, _attackPoint.rotation);
            newProjectile.SetOwner(gameObject);
            newProjectile.direction = _attackPoint.forward;
            
            NetworkObject networkedProjectile = newProjectile.GetComponent<NetworkObject>();
            networkedProjectile.Spawn(true);

            _lastShootTime = Time.time;
        }
        
    }
}
