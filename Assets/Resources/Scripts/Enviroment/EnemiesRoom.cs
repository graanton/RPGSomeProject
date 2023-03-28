using UnityEngine;
using UnityEngine.Events;

public class EnemiesRoom : LockedRoomBase
{
    [SerializeField] private EnemiesSpawner _enemiesSpawner;

    private int _enemyCount = 0;
    private bool _isLocked = false;

    public void Awake()
    {
        _enemiesSpawner.spawnEvent.AddListener(OnEnemySpawend);
        OpenEvent.AddListener(() => _isLocked = false);
        LockEvent.AddListener(() => _isLocked = true);
    }

    private void OnEnemySpawend(Enemy enemy)
    {
        _enemyCount++;
        enemy.OnDeath.AddListener(OnEnemyDead);
    }

    private void OnEnemyDead()
    {
        _enemyCount--;
        if (_enemyCount == 0)
        {
            Open();
        }
    }

    public override bool IsLocked()
    {
         return _isLocked;
    }
}
