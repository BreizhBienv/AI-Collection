using System.Collections.Generic;
using UnityEngine;

public class MoveToPickaxe_Action : BaseAction
{
    public MoveToPickaxe_Action()
    {
        _conditions.Add(EWorldState.AVAILABLE_PICKAXE,  true);
        _conditions.Add(EWorldState.NEAR_PICKAXE,       false);
        _conditions.Add(EWorldState.HAS_PICKAXE,        false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWorldState = new(pSimulated)
        {
            [EWorldState.NEAR_PICKAXE]  = true,
            [EWorldState.NEAR_CHEST]    = false,
            [EWorldState.NEAR_CHUNK]    = false,
            [EWorldState.NEAR_FURNACE]  = false,
        };

        return newWorldState;
    }
    public override void StartAction(MinerAgent pAgent)
    {
        pAgent._perceivedWorldState[EWorldState.NEAR_CHEST]     = false;
        pAgent._perceivedWorldState[EWorldState.NEAR_CHUNK]     = false;
        pAgent._perceivedWorldState[EWorldState.NEAR_FURNACE]   = false;
    }

    public override void Execute(MinerAgent pAgent)
    {
        Pickaxe pickaxe = pAgent._target?.GetComponent<Pickaxe>();
        if (pickaxe != null && World.Instance.IsPickaxeAvailable(pickaxe))
            return;

        List<Pickaxe> pickaxes = World.Instance._pickaxes;
        if (pickaxes.Count <= 0)
            return;

        pAgent._target = World.Instance.GetRandomPickaxe().gameObject;
        pAgent._navMeshAgent.SetDestination(pAgent._target.transform.position);

        Debug.Log("Moving To " + pAgent._target.name);
    }

    public override bool IsComplete(MinerAgent pAgent, float pTimeInAction)
    {
        if (pAgent._target == null || !pAgent.CloseEnoughToTarget())
            return false;

        Debug.Log("Reached " + pAgent._target.name);
        return true;
    }

    public override void FinishAction(MinerAgent pAgent)
    {
        pAgent._perceivedWorldState[EWorldState.NEAR_PICKAXE] = true;
    }

    public override void AbortAction(MinerAgent pAgent)
    {
    }
}
