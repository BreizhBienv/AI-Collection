using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class MinerAgent : MonoBehaviour
{
    public NavMeshAgent _navMeshAgent { get; private set; }

    [NonSerialized] public int _numOrePossesed;

    private Dictionary<EWorldState, bool> _goal;

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _goal = new Dictionary<EWorldState, bool>
        {
            //{ EWorldState.GOAL_INGOT_DELIVERED, true },
            //{ EWorldState.AVAILABLE_FURNACE,    false }
            { EWorldState.NEAR_CHUNK, true }
        };
        
    }

    protected void Start()
    {
        StartCoroutine(BuildGraph());
    }

    protected virtual void Update()
    {

    }

    private IEnumerator BuildGraph()
    {
        yield return new WaitForSeconds(0.5f);

        List<Node> leaves = new List<Node>();
        Node root = new Node(new(World.Instance._worldState));
        List<BaseAction> actions = new List<BaseAction>()
        {
            { new MoveToOre_Action() },
            { new MineOre_Action() },
        };

        Miner_GOAP.BuildGraph(root, leaves, actions, _goal);

        if (leaves.Count <= 0)
            yield break;

        leaves = leaves.OrderBy(item => item._totalCost).ToList();
        Debug.Log("Leaves Number: " + leaves.Count);

        foreach (BaseAction action in leaves[0]._actions)
        {
            action.Execute();
        }
    }

    private void CompleteAction()
    {
    }

    private void AboartAction()
    {

    }

    private void EndAction()
    {

    }
}
