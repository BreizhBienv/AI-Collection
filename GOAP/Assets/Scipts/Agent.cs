using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour
{
    public NavMeshAgent _navMeshAgent { get; private set; }

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
