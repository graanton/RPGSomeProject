using UnityEngine;
using UnityEngine.Events;

public class EnemiesRoom : Room
{
    [SerializeField] private EnemiesSpawner _enemiesSpawner;

    public UnityEvent allEnemiesDeadEvent = new();

    private int _enemyCount = 0;

    public void Awake()
    {
        _enemiesSpawner.spawnEvent.AddListener(OnEnemySpawend);
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
            allEnemiesDeadEvent?.Invoke();
        }
    }
}
