using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class JerkButton : MonoBehaviour
{
    [SerializeField] private Jerkble _jerkble;

    private Button _button;

    public void SetTarget(NetworkObject target)
    {
        _jerkble = target.GetComponent<Jerkble>();
        _button.onClick.AddListener(_jerkble.Jerk);
    }

    private void Start()
    {
        _button = GetComponent<Button>();
        if (_jerkble != null )
        {
            _button.onClick.AddListener(_jerkble.Jerk);
        }
    }
}
