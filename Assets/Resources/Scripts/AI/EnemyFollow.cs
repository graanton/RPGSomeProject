using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyFollow : NetworkBehaviour
{
    [SerializeField] private Enemy _targetDetecter;

    private NavMeshAgent _agent;
    private HashSet<PlayerHealth> _targets = new();

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = false;
        _targetDetecter.haveTargetEvent.AddListener(AddTarget); 
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            _agent.enabled = true;
        }
    }

    private void Update()
    {
        if (IsServer && _targets.Count > 0)
        {
            Transform target = GetNeargestTarget();

            _agent.SetDestination(target.position);
        }
    }

    private void AddTarget(PlayerHealth target)
    {
        _targets.Add(target);
    }

    private Transform GetNeargestTarget()
    {
        if (_targets.Count == 0)
        {
            Debug.LogWarning("Nothing players");
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
}
