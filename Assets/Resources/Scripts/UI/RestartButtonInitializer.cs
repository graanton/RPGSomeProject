using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using IJunior.TypedScenes;

public class RestartButtonInitializer : MonoBehaviour
{
    [SerializeField] private Button _reloadButton;

    private void Start()
    {
        _reloadButton.onClick.AddListener(() => StartMenu.Load());
        if (NetworkManager.Singleton != null)
            _reloadButton.onClick.AddListener(() => NetworkManager.Singleton.Shutdown());
    }
}
