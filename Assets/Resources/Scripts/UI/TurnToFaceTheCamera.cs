using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToFaceTheCamera : MonoBehaviour
{
    private Camera _camera;

    private const float FACE_ROTATION_ANGLE = 180;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.LookAt(new Vector3(transform.position.x,
            _camera.transform.position.y, _camera.transform.position.z));
        transform.Rotate(Vector3.up, FACE_ROTATION_ANGLE);
    }
}
