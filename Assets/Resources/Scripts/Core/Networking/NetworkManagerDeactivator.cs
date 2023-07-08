using IJunior.TypedScenes;
using Unity.Netcode;
using UnityEngine;

public class NetworkManagerDeactivator : MonoBehaviour, ISceneLoadHandler<bool>
{
    [SerializeField] private NetworkManager _networkManager;

    public void OnSceneLoaded(bool isOnlineMode)
    {
        _networkManager.enabled = isOnlineMode;
    }
}
