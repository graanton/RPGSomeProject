using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Jerkble : MonoBehaviour
{
    [SerializeField] private AnimationCurve _distance;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Jerk()
    {
        print("dash");
        StartCoroutine(Jerking(transform.forward));
    }

    private IEnumerator Jerking(Vector3 direction)
    {
        float startTime = Time.time;
        float endTime = startTime +
            _distance.keys[_distance.keys.Length - 1].time;
        float currentTime = startTime;
        Vector3 currentDirection = Vector3.zero;
        Vector3 previousPosition = currentDirection;

        while (currentTime < endTime)
        {
            currentDirection = direction *
                _distance.Evaluate(currentTime - startTime) -
                previousPosition;

            _rigidbody.MovePosition(_rigidbody.position + currentDirection);
            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;
            previousPosition = currentDirection;
        }
    }
}
