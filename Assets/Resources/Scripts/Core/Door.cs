using System;
using Unity.Netcode;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private LockedRoomBase _room;

    public event Action openEvent;
    public event Action closeEvent;

    private bool _hasNeighbore = false;

    private void Awake()
    {
        _room.OpenEvent += Open;
        _room.LockEvent += Close;
        Close();
    }

    public void Close()
    {
        closeEvent?.Invoke();
    }

    public void Open()
    {
        if (_hasNeighbore)
            openEvent?.Invoke();
    }
}
