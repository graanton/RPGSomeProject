using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar: IDisposable
{
    private Health _health;
    private Image _healthBarFillImage;
    private Gradient _healthBarGradient;

    public HealthBar(Health health, Image healthBarFillImage, Gradient healthBarGradient)
    {
        _health = health;
        _healthBarFillImage = healthBarFillImage;
        _healthBarGradient = healthBarGradient;
        _health.HitEvent += OnHit;
    }

    public void Dispose()
    {
        _health.HitEvent -= OnHit;
    }

    private void OnHit(int damage)
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float healthPrecentage = (float)_health.CurrentHealth / _health.MaxHealth;
        _healthBarFillImage.fillAmount = healthPrecentage;
        _healthBarFillImage.color = _healthBarGradient.Evaluate(healthPrecentage);
    }
}
