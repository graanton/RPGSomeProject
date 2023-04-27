using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Image _healthBarFillImage;
    [SerializeField] private Gradient _healthBarGradient;

    private void Awake()
    {
        if (_health != null)
        {
            _health.HitEvent.AddListener(OnHit);
            OnHit(0);
        }
    }

    private void OnHit(int damage)
    {
        float healthPrecentage = (float)_health.CurrentHealth / _health.MaxHealth;
        _healthBarFillImage.fillAmount = healthPrecentage;
        _healthBarFillImage.color = _healthBarGradient.Evaluate(healthPrecentage);
    }

    public void SetTarget(NetworkObject target)
    {
        _health = target.GetComponent<Health>();
        Awake();
    }
}
