using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private EnemyPoint[] _points;
    [SerializeField] private IncomingAndOutgoingWatcher _targetDetecter;

    private List<Enemy> _spawnedEnemies = new();

    public EnemySpawnEvent spawnEvent = new();

    private void Start()
    {
        foreach (EnemyPoint point in _points)
        {
            Enemy instanceEnemy = Instantiate(point.enemyPrefab,
                point.root.position, point.root.rotation);
            instanceEnemy.SetTargetDetecter(_targetDetecter);

            spawnEvent?.Invoke(instanceEnemy);
            _spawnedEnemies.Add(instanceEnemy);
        }
    }

    private void OnDestroy()
    {
        foreach(Enemy enemy in _spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}

[Serializable]
public struct EnemyPoint
{
    public Enemy enemyPrefab;
    public Transform root;
}

[Serializable]
public class EnemySpawnEvent: UnityEvent<Enemy> { }