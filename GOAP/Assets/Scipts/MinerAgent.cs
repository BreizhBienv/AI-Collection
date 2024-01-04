using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(NavMeshAgent))]
public class MinerAgent : MonoBehaviour
{
    public NavMeshAgent _navMeshAgent { get; private set; }

    [NonSerialized] public int _orePossesed;
    [NonSerialized] public int _ingotPossesed;

    private Dictionary<EWorldState, bool> _perceivedWorldState;

    private Dictionary<EWorldState, bool> _goal;
    private List<BaseAction> _actions;

    [NonSerialized] public GameObject _target = null;

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _goal = new Dictionary<EWorldState, bool>
        {
            { EWorldState.STORE_INGOT, true },
            //{ EWorldState.AVAILABLE_FURNACE,    false }
        };

        _perceivedWorldState = new Dictionary<EWorldState, bool>()
        {
            { EWorldState.NEAR_CHUNK,   false },
            { EWorldState.NEAR_FURNACE, false },
            { EWorldState.NEAR_CHEST,   false },
            { EWorldState.HAS_ORES,     false },
            { EWorldState.HAS_INGOTS,   false },
            { EWorldState.STORE_INGOT,  false },
        };

        _actions = new List<BaseAction>()
        {
            { new MoveToOre_Action(MoveToOreChunk) },
            { new FurnaceToProcess_Action(MoveToFurnace) },
            { new FurnaceToRetrieve_Action(MoveToFurnace) },
            { new MoveToChest_Action(MoveToChest) },

            { new MineOre_Action(MineOreChunk) },
            { new ProcessOre_Action(ProcessOre) },

            { new RetrieveIngot_Action(RetrieveIngot) },
            { new StoreIngot_Action(StoreIngot) },
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

        Dictionary<EWorldState, bool> mergedWorldState = new(World.Instance._worldState);
        _perceivedWorldState.ToList().ForEach(x => mergedWorldState.Add(x.Key, x.Value));

        Node root = new Node(mergedWorldState);
        List<Node> leaves = new List<Node>();

        Miner_GOAP.BuildGraph(root, leaves, _actions, _goal);

        if (leaves.Count <= 0)
            yield break;

        leaves = leaves.OrderBy(item => item._totalCost).ToList();
        Debug.Log("Leaves Number: " + leaves.Count);

        StartCoroutine(ExecutePlan(leaves[0]._actions));
    }

    private IEnumerator ExecutePlan(List<BaseAction> pPlan)
    {
        while (pPlan.Count > 0)
        {
            BaseAction action = pPlan[0];
            
            if (!action.IsValid(new()))
            {
                Abort();
                yield break;
            }

            action.Execute(this);

            if (!action.IsComplete(this))
                continue;

            pPlan.RemoveAt(0);
        }   
    }

    private void Abort()
    {

    }

    public bool CloseEnoughToTarget()
    {
        return _navMeshAgent.remainingDistance <= Utils.distanceToTarget;
    }

    #region Actions
    private void MoveToOreChunk()
    {

    }

    private void MoveToFurnace()
    {
    }

    private void MoveToChest()
    {
    }

    private void MineOreChunk()
    {
    }

    private void ProcessOre()
    {
    }

    private void RetrieveIngot()
    {
    }

    private void StoreIngot()
    {
    }
    #endregion
}
