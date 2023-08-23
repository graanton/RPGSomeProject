using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesRoom : LockedRoomBase
{
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
    [SerializeField] private IncomingAndOutgoingWatcher _incomingAndOutgoingWatcher;

    private int _enemiesCount = 0;
    private List<Enemy> _enemies= new List<Enemy>();
    private bool _isLocked = false;

    public override event Action OpenEvent;
    public override event Action LockEvent;

    public IReadOnlyCollection<Enemy> Enemies => _enemies;

    public void Awake()
    {
        _enemiesSpawner.spawnEvent.AddListener(OnEnemySpawend);
        OpenEvent += () => _isLocked = false;
        LockEvent += () => _isLocked = true;
        _incomingAndOutgoingWatcher.enterEvent.AddListener(OnPlayerEnter);
    }

    private void OnPlayerEnter(Health player)
    {
        TryLock();
    }

    private void OnEnemySpawend(Enemy enemy)
    {
        _enemies.Add(enemy);
        _enemiesCount++;
        enemy.DeathEvent += () => OnEnemyDead(enemy);
    }

    private void OnEnemyDead(Enemy enemy)
    {
        _enemies.Remove(enemy);
        _enemiesCount--;
        if (_enemiesCount == 0)
        {
            TryOpen();
        }
    }

    public override bool TryLock()
    {
        if (_enemiesCount > 0 && !_isLocked)
        {
            LockEvent?.Invoke();
            return true;
        }
        return false;
    }

    public override bool IsLocked()
    {
         return _isLocked;
    }
}
