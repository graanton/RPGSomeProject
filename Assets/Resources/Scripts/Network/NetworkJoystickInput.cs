using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(NetworkObject))]
public class NetworkJoystickInput : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        Debug.Log(IsOwner);
    }
}
