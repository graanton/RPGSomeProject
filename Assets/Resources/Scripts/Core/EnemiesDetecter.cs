using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesDetecter : MonoBehaviour
{
    [SerializeField] private RoomMover _roomMover;

    private List<Enemy> _enemyList = new();

    public IReadOnlyCollection<Enemy> EnemyList => _enemyList;

    private void Awake()
    {
        _roomMover.enterEvent.AddListener(OnEnter);
        _roomMover.exitEvent.AddListener(OnExit);
    }

    private void OnEnter(Room room)
    {
        if (room is EnemiesRoom)
        {
            EnemiesRoom enemiesRoom = (EnemiesRoom)room;
            foreach(Enemy enemy in enemiesRoom.Enemies)
            {
                enemy.DeathEvent += () => _enemyList.Remove(enemy);
            }
            _enemyList.AddRange(enemiesRoom.Enemies);
        }
    }
    
    private void OnExit(Room room)
    {
        if (room is EnemiesRoom)
        {
            EnemiesRoom enemiesRoom = (EnemiesRoom)room;
            foreach(Enemy enemy in enemiesRoom.Enemies)
            {
                _enemyList.Remove(enemy);
            }
        }
    }
}
