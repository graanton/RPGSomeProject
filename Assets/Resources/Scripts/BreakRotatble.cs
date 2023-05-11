using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BreakRotatble : MonoBehaviour
{
    [SerializeField] EnemiesDetecter _detecter;
    [SerializeField, Range(0, 360)] private float _angle = 30;

    public void Rotate(Vector2 direction)
    {
        var enemies = _detecter.EnemyList;
        if (enemies.Count > 0)
        {
            Enemy nearEnemyToAngle = enemies.ElementAt(0);
            float angle = CulculateAngle(nearEnemyToAngle.transform, direction);
            foreach (var enemy in enemies)
            {
                float currentAngle = CulculateAngle(enemy.transform, direction);
                if (currentAngle < angle)
                {
                    angle = currentAngle;
                    nearEnemyToAngle = enemy;
                }
            }
            Vector3 lookDirection = AxisConverter.XYToXZ(direction);
            if (Mathf.Abs(angle) <= _angle / 2)
            {
                lookDirection = AxisConverter.XYZToXZ(
                    nearEnemyToAngle.transform.position - transform.position);
            }
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    private float CulculateAngle(Transform other, Vector2 direction)
    {
        return Vector3.Angle(AxisConverter.XYZToXZ(
                other.position - transform.position),
                AxisConverter.XYToXZ(direction));
    }
}
