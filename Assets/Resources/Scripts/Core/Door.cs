using System;
using Resources.Scripts.Common;
using Unity.Netcode;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private LockedRoomBase _room;
    [SerializeField] private GameObject _openVersion;
    [SerializeField] private GameObject _closeVersion;

    private bool _hasNeighbore = false;

    private const float HALLWAY_DISTANCE_TO_OPEN = 3;

    private void Awake()
    {
        _room.OpenEvent += Open;
        _room.LockEvent += Close;
        _room.ConnectedEvent += OnConnected;
        Close();
    }

    private void OnConnected(Connect connectOnfo)
    {
        foreach (Hallway hallway in connectOnfo.HallwaysPositions.Keys)
        {
            Vector2Int hallwayPosition = connectOnfo.HallwaysPositions[hallway];
            if (Vector2Int.Distance(hallwayPosition, AxisConverter.XZToXYInt(transform.position)) <=
                HALLWAY_DISTANCE_TO_OPEN)
            {
                _hasNeighbore = true;
                if (!_room.IsLocked())
                {
                    Open();
                }
                break;
            }
        }
    }

    public void Close()
    {
        _closeVersion.SetActive(true);
        _openVersion.SetActive(false);
    }

    public void Open()
    {
        if (_hasNeighbore)
        {
            _closeVersion.SetActive(false);
            _openVersion.SetActive(true);
        }
    }
}
