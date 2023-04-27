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
        if (target.TryGetComponent(out Jerkble jerkble))
        {
            _jerkble = jerkble;
            _button.onClick.AddListener(_jerkble.Jerk);
        }
        else
        {
            Debug.LogWarning("Missing Jerkble component");
            return;
        }
    }

    private void Start()
    {
        _button = GetComponent<Button>();
        if (_jerkble != null)
        {
            _button.onClick.AddListener(_jerkble.Jerk);
        }
    }
}
