using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class RestartButtonInitializer : MonoBehaviour
{
    [SerializeField] private Button _reloadButton;

    private void Start()
    {
        _reloadButton.onClick.AddListener(Disconnect);
        _reloadButton.onClick.AddListener(SceneReloadble.ReloadScene); 
    }

    private void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
    }
}
