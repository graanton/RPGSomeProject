using System;
using Unity.Netcode;
using UnityEngine;

public class PlayersTeleporterToRoom : MonoBehaviour
{
    [SerializeField] private PlayerRegister _playersRegister;

    private void Awake()
    {
        _playersRegister.registerEvent.AddListener(OnPlayerRigistered);
    }

    private void OnPlayerRigistered(NetworkObject player)
    {
        if (player.TryGetComponent(out RoomMover roomMover))
        {
            roomMover.enterEvent.AddListener(
                delegate(Room room) { TeleportPlayers(player, room); });
        }
    }

    private void TeleportPlayers(NetworkObject playerToTeleport, Room room)
    {
        foreach (NetworkObject player in _playersRegister.Players)
        {
            if (player != playerToTeleport)
            {
                player.transform.position = playerToTeleport.transform.position;
            }
        }
    }
}
