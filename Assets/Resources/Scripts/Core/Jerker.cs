using UnityEngine;
using Zenject;

public class Jerker : MonoBehaviour
{
    [SerializeField] private HoldButton _button;

    private Jerkble _jerkble;

    private void Awake()
    {
        _button.PressedEvent += OnPressed;
    }

    [Inject]
    private void Construct(PlayerHealth playerHealth)
    {
        _jerkble = playerHealth.GetComponent<Jerkble>();
    }

    private void OnPressed()
    {
        _jerkble.Jerk();
    }
}
