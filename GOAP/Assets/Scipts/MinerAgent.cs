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
    private List<BaseAction> _actions;

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _goal = new Dictionary<EWorldState, bool>
        {
            { EWorldState.INGOT_DELIVERED, true },
            //{ EWorldState.AVAILABLE_FURNACE,    false }
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
            { new DeliverIngot_Action(DeliverIngot) },
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

        Miner_GOAP.BuildGraph(root, leaves, _actions, _goal);

        if (leaves.Count <= 0)
            yield break;

        leaves = leaves.OrderBy(item => item._totalCost).ToList();
        Debug.Log("Leaves Number: " + leaves.Count);

        foreach (BaseAction action in leaves[0]._actions)
        {
            action.Execute();
        }
    }

    #region Actions
    private void MoveToOreChunk()
    {
        Debug.Log("Moved To Ore");
    }

    private void MoveToFurnace()
    {
        Debug.Log("Moved To Furnace");
    }

    private void MoveToChest()
    {
        Debug.Log("Moved To Chest");
    }

    private void MineOreChunk()
    {
        Debug.Log("Mined Ore");
    }

    private void ProcessOre()
    {
        Debug.Log("Processed Ore");
    }

    private void RetrieveIngot()
    {
        Debug.Log("Ingot retrieved");
    }

    private void DeliverIngot()
    {
        Debug.Log("Delivered Ingot");
    }
    #endregion

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
