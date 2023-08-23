using Unity.Netcode;
using UnityEngine;

public class PlayersTeleporterToRoom : MonoBehaviour
{
    [SerializeField] private SpawnedPlayersRegister _playersRegister;

    private void Awake()
    {
        _playersRegister.registerEvent.AddListener(OnPlayerRigistered);
    }

    private void OnPlayerRigistered(NetworkObject player)
    {
        if (player.TryGetComponent(out RoomMover roomMover))
        {
            roomMover.enterEvent += (Room room) => TeleportPlayers(player); 
        }
    }

    private void TeleportPlayers(NetworkObject playerToTeleport)
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
