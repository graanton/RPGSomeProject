using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class EnemiesSpawner : NetworkBehaviour
{
    [SerializeField] private EnemyPoint[] _points;
    [SerializeField] private IncomingAndOutgoingWatcher _targetDetecter;

    private List<Enemy> _spawnedEnemies = new();

    public EnemySpawnEvent spawnEvent = new();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            foreach (EnemyPoint point in _points)
            {
                Enemy instanceEnemy = Instantiate(point.enemyPrefab,
                    point.root.position, point.root.rotation);
                instanceEnemy.SetTargetDetecter(_targetDetecter);

                NetworkObject networkedEnemy = instanceEnemy.GetComponent<NetworkObject>();
                networkedEnemy.Spawn(true);
                networkedEnemy.TrySetParent(point.root);
                spawnEvent?.Invoke(instanceEnemy);
                _spawnedEnemies.Add(instanceEnemy);
            }
        }
    }

    public override void OnDestroy()
    {
        if (IsServer)
        {
            foreach(Enemy enemy in _spawnedEnemies)
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