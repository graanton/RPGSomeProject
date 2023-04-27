using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemiesRoom : LockedRoomBase
{
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
    [SerializeField] private IncomingAndOutgoingWatcher _incomingAndOutgoingWatcher;

    private int _enemiesCount = 0;
    private bool _isLocked = false;

    public void Awake()
    {
        _enemiesSpawner.spawnEvent.AddListener(OnEnemySpawend);
        OpenEvent.AddListener(() => _isLocked = false);
        LockEvent.AddListener(() => _isLocked = true);
        _incomingAndOutgoingWatcher.enterEvent.AddListener(OnPlayerEnter);
    }

    private void OnPlayerEnter(Health player)
    {
        Lock();
    }

    private void OnEnemySpawend(Enemy enemy)
    {
        _enemiesCount++;
        enemy.DeathEvent.AddListener(OnEnemyDead);
    }

    private void OnEnemyDead()
    {
        _enemiesCount--;
        if (_enemiesCount == 0)
        {
            Open();
        }
    }

    public override void Lock()
    {
        if (_enemiesCount > 0)
            base.Lock();
    }

    public override bool IsLocked()
    {
         return _isLocked;
    }
}
