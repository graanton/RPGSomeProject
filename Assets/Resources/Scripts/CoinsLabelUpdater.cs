using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CoinsLabelUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private PlayerSpawner _spawner;

    private Wallet _target;

    private void Awake()
    {
        _spawner.PlayerSpawnEvent.AddListener(OnSpawn);
    }

    private void OnSpawn(NetworkObject player)
    {
        if (player.TryGetComponent(out Wallet wallet))
        {
            _target = wallet;
            UpdateValue();
            wallet.AddEvent.AddListener((int arg) => UpdateValue());
            wallet.GrabEvent.AddListener((int arg) => UpdateValue());
        }
    }

    private void UpdateValue()
    {
        _label.text = $"{_target.Ammount}";
    }
}
