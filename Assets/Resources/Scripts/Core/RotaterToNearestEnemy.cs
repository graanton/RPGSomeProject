using System.Linq;
using UnityEngine;

public class RotaterToNearestEnemy : MonoBehaviour
{
    [SerializeField] private EnemiesDetecter _detecter;
    [SerializeField] private bool _autoRotate = true;

    private void Update()
    {
        if (_autoRotate)
        {
            Rotate();
        }
    }

    public void Rotate()
    {
        Enemy nearestEnemy = GetNearestEnemy();
        if (nearestEnemy != null)
        {
            Vector3 position = AxisConverter.XYZToXZ(transform.position);
            Vector3 enemyPosition = AxisConverter.XYZToXZ(nearestEnemy.transform.position);
            Vector3 directionToEnemy = enemyPosition - position;

            transform.rotation = 
                Quaternion.LookRotation(directionToEnemy.normalized);
        }
    }

    public Enemy GetNearestEnemy()
    {
        if (_detecter.EnemyList.Count == 0 )
            return null;

        Enemy nearestEnemy = _detecter.EnemyList.ElementAt(0);
        foreach (Enemy enemy in _detecter.EnemyList)
        {
            if (DistanceFromSelf(enemy.transform) <
                DistanceFromSelf(nearestEnemy.transform))
            {
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    private float DistanceFromSelf(Transform other)
    {
        return Vector3.Distance(transform.position, other.position);
    }
}
