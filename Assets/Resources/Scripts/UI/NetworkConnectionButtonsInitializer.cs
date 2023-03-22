using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;


public class NetworkConnectionButtonsInitializer : MonoBehaviour
{
    [SerializeField] private Button _client, _host, _server;

    private void Start()
    {
        _client.onClick.AddListener(StartClient);
        _host.onClick.AddListener(StartHost);
        _server.onClick.AddListener(StartServer);
    }

    private void StartClient() => NetworkManager.Singleton.StartClient();
    private void StartHost() => NetworkManager.Singleton.StartHost();
    private void StartServer() => NetworkManager.Singleton.StartServer();
}
