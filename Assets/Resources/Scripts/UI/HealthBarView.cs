using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Image _image;
    [SerializeField] private Gradient _gradient;

    private void Start()
    {
        HealthBar healthBar = new(_health, _image, _gradient);
        _health.DeathEvent += healthBar.Dispose;
    }
}
