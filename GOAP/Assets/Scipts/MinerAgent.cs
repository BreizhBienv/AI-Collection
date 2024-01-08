using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;
using System.Collections;
using Unity.VisualScripting;
using System.Data;

[RequireComponent(typeof(NavMeshAgent))]
public class MinerAgent : MonoBehaviour
{
    public NavMeshAgent _navMeshAgent { get; private set; }

    [NonSerialized] public int _orePossesed;
    [NonSerialized] public int _ingotPossesed;

    public Dictionary<EWorldState, bool> _perceivedWorldState;

    private List<BaseAction> _craftIngotActions;
    private List<BaseAction> _storeIngotActions;

    [NonSerialized] public GameObject _target = null;

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.stoppingDistance = Utils.stoppingDistance;

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
            { new MoveToOre_Action() },
            { new FurnaceToProcess_Action() },

            { new MineOre_Action() },
            { new ProcessOre_Action() },
        };

        _storeIngotActions = new List<BaseAction>()
        {
            { new FurnaceToRetrieve_Action() },
            { new MoveToChest_Action() },

            { new RetrieveIngot_Action() },
            { new StoreIngot_Action() },
        };
    }

    protected void Start()
    {
        StartCoroutine(BuildGraph());
    }

    private IEnumerator BuildGraph()
    {
        yield return null;

        Dictionary<EWorldState, bool> mergedWorldState = new(World.Instance._worldState);
        _perceivedWorldState.ToList().ForEach(x => mergedWorldState.Add(x.Key, x.Value));

        Node root = new Node(mergedWorldState);
        List<Node> leaves = new List<Node>();

        Dictionary<EWorldState, bool> goal = null;
        List<BaseAction> actions = null;

        GetBestPlan(ref goal, ref actions);

        Miner_GOAP.BuildGraph(root, ref leaves, actions, goal);

        if (leaves.Count <= 0)
        {
            StartCoroutine(Sleep());
            yield break;
        }

        leaves = leaves.OrderBy(item => item._totalCost).ToList();
        Debug.Log("Leaves Number: " + leaves.Count);

        StartCoroutine(ExecutePlan(leaves[0]._actions));
    }

    private void GetBestPlan(ref Dictionary<EWorldState, bool> pGoal, ref List<BaseAction> pActions)
    {
        bool ingotAvailable = World.Instance._worldState[EWorldState.AVAILABLE_INGOT];
        bool furnaceAvailable = World.Instance._worldState[EWorldState.AVAILABLE_FURNACE];

        if (ingotAvailable)
        {
            pActions = new(_storeIngotActions);
            pGoal = new Dictionary<EWorldState, bool>()
            {
                { EWorldState.STORE_INGOT, true },
            };
        }
        else if (furnaceAvailable)
        {
            pActions = new(_craftIngotActions);
            pGoal = new Dictionary<EWorldState, bool>()
            {
                { EWorldState.PROCESS_ORE, true },
            };
        }
    }

    private IEnumerator Sleep()
    {
        while (true)
        {
            yield return 0;

            bool ingotAvailable = World.Instance._worldState[EWorldState.AVAILABLE_INGOT];
            bool furnaceAvailable = World.Instance._worldState[EWorldState.AVAILABLE_FURNACE];
            bool chunkAvailable = World.Instance._worldState[EWorldState.AVAILABLE_CHUNK];

            if (!ingotAvailable && !furnaceAvailable && !chunkAvailable)
                continue;
            
            StartCoroutine(BuildGraph());
            yield break;
            
        }
    }

    private IEnumerator ExecutePlan(List<BaseAction> pPlan)
    {
        bool hasStartedAction = false;

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

            if (!hasStartedAction)
            {
                hasStartedAction = true;
                action.StartAction(this);
            }

            action.Execute(this);


            if (!action.IsComplete(this))
            {
                yield return 0;
                continue;
            }

            hasStartedAction = false;
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
