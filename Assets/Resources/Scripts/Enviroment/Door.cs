using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Door : Tile3dBase
{
    [SerializeField] private LockedRoomBase _room;

    public UnityEvent openEvent = new();
    public UnityEvent closeEvent = new();

    private NetworkVariable<bool> _hasNeighbore = new(false);

    private void Awake()
    {
        BoundsInit();
        _room.HallwayAddEvent.AddListener(OnNeighboreAdded);
        _room.OpenEvent.AddListener(Open);
        _room.LockEvent.AddListener(Close);
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            if (_room.IsLocked())
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }

    private void OnNeighboreAdded(Room neighbore)
    {
        bool isMyNeighbore = neighbore.OnTheBuffer(LocalBounds)
             &&
            Vector2Int.Distance(LocalBounds.position,
            neighbore.LocalBounds.position) == 1;

        if (isMyNeighbore)
        {
            _hasNeighbore.Value = true;
            Open();
        }
    }

    public void Close()
    {
        closeEvent?.Invoke();
    }

    public void Open()
    {
        if (_hasNeighbore.Value)
            openEvent?.Invoke();
    }
}
