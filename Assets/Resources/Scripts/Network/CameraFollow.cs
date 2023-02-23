using Unity.Netcode;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;

    private Transform _target;

    public void SetTaget(NetworkObject target)
    {
        if (target.OwnerClientId == NetworkManager.Singleton.LocalClientId)
        {
            _target = target.transform;
        }
    }

    private void Update()
    {
        if (_target != null)
        {
            transform.position = _target.position + _offset;
        }
    }
}
