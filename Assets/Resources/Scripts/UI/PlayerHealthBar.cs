using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBarFillImage;
    [SerializeField] private Gradient _healthBarGradient;

    private PlayerHealth _player;

    [Inject]
    private void Construct(PlayerHealth player)
    {
        _player = player;
    }

    private void Start()
    {
        HealthBar healthBar = new(_player, _healthBarFillImage, _healthBarGradient);
        _player.DeathEvent += healthBar.Dispose;
    }
}
