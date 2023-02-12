using Unity.Netcode;
using UnityEngine;

public class CameraFollow : NetworkBehaviour
{
    [SerializeField] private Vector3 _offset;

    private Transform _target;

    public void SetTaget(NetworkObject target)
    {
        _target = target.transform;
    }

    private void Update()
    {
        if (_target != null)
        {
            transform.position = _target.position + _offset;
        }
    }
}
