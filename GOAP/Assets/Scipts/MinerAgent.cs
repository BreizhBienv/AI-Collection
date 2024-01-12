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

    [NonSerialized] public GameObject _target = null;
    [NonSerialized] public int _orePossesed;
    [NonSerialized] public int _ingotPossesed;

    public Dictionary<EWorldState, bool> _perceivedWorldState;

    private List<BaseAction> _craftIngotActions;
    private List<BaseAction> _storeIngotActions;

    public GameObject _PickaxeSlot;

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.stoppingDistance = Utils.stoppingDistance;

        _perceivedWorldState = new Dictionary<EWorldState, bool>()
        {
            { EWorldState.NEAR_CHUNK,   false },
            { EWorldState.NEAR_FURNACE, false },
            { EWorldState.NEAR_CHEST,   false },
            { EWorldState.NEAR_PICKAXE, false },
            { EWorldState.HAS_ORES,     false },
            { EWorldState.HAS_INGOTS,   false },
            { EWorldState.HAS_PICKAXE,  false },
            { EWorldState.PROCESS_ORE,  false },
            { EWorldState.STORE_INGOT,  false },
        };

        _craftIngotActions = new List<BaseAction>()
        {
            { new MoveToPickaxe_Action() },
            { new MoveToOre_Action() },
            { new FurnaceToProcess_Action() },

            { new EquipPickaxe_Action() },
            //{ new MineOre_Action() },
            { new MineWithHands_Action() },
            { new MineWithPickaxe_Action() },
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

            if (!ingotAvailable || (!furnaceAvailable && !chunkAvailable))
                continue;

            StartCoroutine(BuildGraph());
            yield break;
        }
    }

    private IEnumerator ExecutePlan(List<BaseAction> pPlan)
    {
        bool hasStartedAction = false;
        float timeInAction = 0f;

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
            timeInAction += Time.deltaTime;

            if (!action.IsComplete(this, timeInAction))
            {
                yield return 0;
                continue;
            }

            hasStartedAction = false;
            timeInAction = 0f;
            action.FinishAction(this);
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

    public void EquipPickaxe(Pickaxe pPickaxe)
    {
        if (pPickaxe == null || !World.Instance.IsPickaxeAvailable(pPickaxe))
            return;

        pPickaxe.OnUnavailable();
        pPickaxe.transform.SetParent(_PickaxeSlot.transform);
        pPickaxe.transform.localPosition = Vector3.zero;
        pPickaxe.transform.rotation = Quaternion.identity;
    }

    public void UnequipPickaxe()
    {
        Pickaxe pickaxe = _PickaxeSlot.GetComponentInChildren<Pickaxe>();
        if (pickaxe == null) 
            return;

        pickaxe.OnAvailable();
        pickaxe.gameObject.transform.SetParent(null);
        _perceivedWorldState[EWorldState.HAS_PICKAXE] = false;

        pickaxe.Impulse();
    }
}
