using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Jerkble : MonoBehaviour
{
    [SerializeField] private AnimationCurve _distance;

    public UnityEvent StartJerkEvent = new();
    public UnityEvent StopJerkEvent = new();

    private Rigidbody _rigidbody;
    private bool _isJerking = false;

    private void Start()
    {
        StartJerkEvent.AddListener(() => _isJerking = true);
        StopJerkEvent.AddListener(() => _isJerking = false);
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Jerk()
    {
        if (_isJerking)
        {
            Debug.LogWarning("Is jerking");
            return;
        }
        else
        {
            StartCoroutine(Jerking(transform.forward));
        }
    }

    private IEnumerator Jerking(Vector3 direction)
    {
        float startTime = Time.time;
        float endTime = startTime +
            _distance.keys[_distance.keys.Length - 1].time;
        float currentTime = startTime;
        StartJerkEvent?.Invoke();
        while (currentTime <= endTime)
        {
            float distanceToMove = _distance.Evaluate(currentTime - startTime) -
            _distance.Evaluate(currentTime - startTime - Time.deltaTime);
            Vector3 currentDirection = direction * distanceToMove;
            _rigidbody.MovePosition(_rigidbody.position + currentDirection);
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        StopJerkEvent?.Invoke();
    }
}
