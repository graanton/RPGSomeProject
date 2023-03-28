using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Door : Tile3dBase
{
    [SerializeField] private LockedRoomBase _room;
    [SerializeField] private IncomingAndOutgoingWatcher _targetWaiter;

    private bool _hasNeighbore = false;

    public UnityEvent openEvent = new();
    public UnityEvent closeEvent = new();

    public void Awake()
    {
        _room.HallwayAddEvent.AddListener(OnNeighboreAdded);
        _targetWaiter.enterEvent.AddListener(OnPlayerEnter);
        Close();
        BoundsInit();
        _room.OpenEvent.AddListener(OnRoomOpened);
    }

    private void OnRoomOpened()
    {
        Open();
    }

    private void OnPlayerEnter(Health player)
    {
        Close();
    }

    private void OnNeighboreAdded(Room neighbore)
    {
        bool isMyNeighbore = neighbore.OnTheBuffer(localBounds)
             &&
            Vector2Int.Distance(localBounds.position,
            neighbore.localBounds.position) == 1;

        if (isMyNeighbore)
        {
            _hasNeighbore = true;
            Open();
        }
    }

    public void Close()
    {
        closeEvent?.Invoke();
    }

    public void Open()
    {
        if (_hasNeighbore)
        {
            openEvent?.Invoke();
        }
    }
}
