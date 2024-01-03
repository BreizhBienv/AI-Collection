using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour
{
    protected NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavMeshAgent { get { return _navMeshAgent; } }

    private GameObject _currentTarget = null;

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
    }

    private void CompleteAction()
    {
    }

    private void AboartAction()
    {
        _currentTarget = null;
    }

    private void EndAction()
    {
    }

    private void LateUpdate()
    {
    }
}
