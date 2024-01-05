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

    public Dictionary<EWorldState, bool> _perceivedWorldState;

    private Dictionary<EWorldState, bool> _goal;
    private List<BaseAction> _craftIngotActions;
    private List<BaseAction> _storeIngotActions;

    [NonSerialized] public GameObject _target = null;

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.stoppingDistance = Utils.stoppingDistance;

        _goal = new Dictionary<EWorldState, bool>
        {
            { EWorldState.STORE_INGOT, true },
        };

        _perceivedWorldState = new Dictionary<EWorldState, bool>()
        {
            { EWorldState.NEAR_CHUNK,   false },
            { EWorldState.NEAR_FURNACE, false },
            { EWorldState.NEAR_CHEST,   false },
            { EWorldState.HAS_ORES,     false },
            { EWorldState.HAS_INGOTS,   false },
            { EWorldState.PROCESS_ORE,  false },
            { EWorldState.STORE_INGOT,  false },
        };

        _craftIngotActions = new List<BaseAction>()
        {
            { new MoveToOre_Action(MoveToOreChunk) },
            { new FurnaceToProcess_Action(MoveToFurnace) },

            { new MineOre_Action(MineOreChunk) },
            { new ProcessOre_Action(ProcessOre) },

            { new RetrieveIngot_Action(RetrieveIngot) },
            { new StoreIngot_Action(StoreIngot) },
        };

        _storeIngotActions = new List<BaseAction>()
        {
            { new FurnaceToRetrieve_Action(MoveToFurnace) },
            { new MoveToChest_Action(MoveToChest) },

            { new RetrieveIngot_Action(RetrieveIngot) },
            { new StoreIngot_Action(StoreIngot) },
        };
    }

    protected void Start()
    {
        StartCoroutine(BuildGraph());
    }

    private IEnumerator BuildGraph()
    {
        yield return 0;

        Dictionary<EWorldState, bool> mergedWorldState = new(World.Instance._worldState);
        _perceivedWorldState.ToList().ForEach(x => mergedWorldState.Add(x.Key, x.Value));

        Node root = new Node(mergedWorldState);
        List<Node> leaves = new List<Node>();

        GetBestPlan(out var goal, out var actions);

        Miner_GOAP.BuildGraph(root, ref leaves, actions, goal);

        if (leaves.Count <= 0)
            yield break;

        leaves = leaves.OrderBy(item => item._totalCost).ToList();
        Debug.Log("Leaves Number: " + leaves.Count);

        StartCoroutine(ExecutePlan(leaves[0]._actions));
    }

    private void GetBestPlan(out Dictionary<EWorldState, bool> oGoal, out List<BaseAction> oActions)
    {
        bool ingotAvailable = World.Instance._worldState[EWorldState.AVAILABLE_INGOT];
        bool furnaceAvailable = World.Instance._worldState[EWorldState.AVAILABLE_FURNACE];

        oGoal = null;
        oActions = null;

        if (ingotAvailable)
        {
            oActions = _storeIngotActions;
            oGoal = new Dictionary<EWorldState, bool>()
            {
                { EWorldState.STORE_INGOT, true },
            };


        }
        else if (furnaceAvailable)
        {
            oActions = _craftIngotActions;
            oGoal = new Dictionary<EWorldState, bool>()
            {
                { EWorldState.PROCESS_ORE, true },
            };
        }
    }

    private IEnumerator ExecutePlan(List<BaseAction> pPlan)
    {
        while (pPlan.Count > 0)
        {
            BaseAction action = pPlan[0];

            Dictionary<EWorldState, bool> mergedWorldState = new(World.Instance._worldState);
            _perceivedWorldState.ToList().ForEach(x => mergedWorldState.Add(x.Key, x.Value));
            if (!action.IsValid(mergedWorldState))
            {
                Abort();
                yield break;
            }

            action.StartAction(this);
            action.Execute(this);


            if (!action.IsComplete(this))
            {
                yield return 0;
                continue;
            }

            action.OnFinished(this);
            pPlan.RemoveAt(0);
        }

        StartCoroutine(BuildGraph());
    }

    private void Abort()
    {
        StartCoroutine(BuildGraph());
    }

    public bool CloseEnoughToTarget()
    {
        if (_navMeshAgent.pathPending)
            return false;

        return _navMeshAgent.remainingDistance <= Utils.stoppingDistance;
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
