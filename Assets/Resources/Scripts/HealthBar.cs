using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerSpawner _spawner;
    [SerializeField] private Health _health;
    [SerializeField] private Image _healthBarFillImage;
    [SerializeField] private Gradient _healthBarGradient;

    private void Awake()
    {
        if (_health != null)
        {
            UpdateHealthBar();
            _health.HitEvent.AddListener(OnHit);
        }
        _spawner.PlayerSpawnEvent.AddListener(OnSpawn);
    }

    private void OnHit(int damage)
    {
        UpdateHealthBar();
    }

    private void OnSpawn(NetworkObject target)
    {
        _health = target.GetComponent<Health>();
        Awake();
    }

    private void UpdateHealthBar()
    {
        float healthPrecentage = (float)_health.CurrentHealth / _health.MaxHealth;
        _healthBarFillImage.fillAmount = healthPrecentage;
        _healthBarFillImage.color = _healthBarGradient.Evaluate(healthPrecentage);
    }
}
