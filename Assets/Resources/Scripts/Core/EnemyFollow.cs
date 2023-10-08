using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private Enemy _targetDetecter;

    private NavMeshAgent _agent;
    private HashSet<Health> _targets = new();

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _targetDetecter.playerEvent.AddListener(AddTarget); 
    }

    private void Update()
    {
        if (_targets.Count > 0)
        {
            Transform target = GetNeargestTarget();

            _agent.SetDestination(target.position);
        }
    }

    private void AddTarget(Health target)
    {
        target.DeathEvent += () => OnPlayerDie(target);
        _targets.Add(target);
    }

    private Transform GetNeargestTarget()
    {
        if (_targets.Count == 0)
        {
            Debug.LogError("Nothing players");
            return null;
        }
        Vector3 position = transform.position;
        Transform nearestTarget = _targets.ElementAt(0).transform;
        foreach (PlayerHealth target in _targets)
        {

            if (Vector3.Distance(position, target.transform.position) < 
                Vector3.Distance(position, nearestTarget.position))
            {
                nearestTarget = target.transform;
            }
        }
        return nearestTarget;
    }

    private void OnPlayerDie(Health player)
    {
        _targets.Remove(player);
    }
}
