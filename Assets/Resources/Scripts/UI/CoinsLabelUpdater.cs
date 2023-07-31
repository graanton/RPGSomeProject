using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class CoinsLabelUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label;

    private Wallet _target;

    [Inject]
    private void Construct(PlayerHealth player)
    {
        _target = player.GetComponent<Wallet>();
    }

    private void Awake()
    {
        _target.AddEvent.AddListener((int arg) => UpdateValue());
        _target.GrabEvent.AddListener((int arg) => UpdateValue());
    }

    private void Start()
    {
        UpdateValue();
    }

    private void UpdateValue()
    {
        _label.text = $"{_target.Ammount}";
    }
}
