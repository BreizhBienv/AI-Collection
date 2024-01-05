using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveToChest_Action : BaseAction
{
    public MoveToChest_Action(Action pAction) : base(pAction)
    {
        _conditions.Add(EWorldState.NEAR_CHEST, false);
        _conditions.Add(EWorldState.HAS_INGOTS, true);
        _conditions.Add(EWorldState.STORE_INGOT, false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWS = new(pSimulated)
        {
            [EWorldState.NEAR_CHEST] = true
        };

        return newWS;
    }

    public override void Execute(MinerAgent pAgent)
    {
        if (pAgent._target == null)
        {
            Debug.Log("Moving To Chest");
            pAgent._target = World.Instance.GetRandomChest().gameObject;
        }
        else
        {
            Chest chest = pAgent._target.GetComponent<Chest>();
            if (chest != null)
                return;

            Debug.Log("Moving To Chest");
            pAgent._target = World.Instance.GetRandomChest().gameObject;
        }

        pAgent._navMeshAgent.SetDestination(pAgent._target.transform.position);
    }

    public override bool IsComplete(MinerAgent pAgent)
    {
        if (pAgent.CloseEnoughToTarget())
        {
            Debug.Log("Moved To Chest");
            return true;
        }

        return false;
    }

    public override void StartAction(MinerAgent pAgent)
    {
        base.StartAction(pAgent);

        pAgent._perceivedWorldState[EWorldState.NEAR_CHUNK] = false;
        pAgent._perceivedWorldState[EWorldState.NEAR_FURNACE] = false;
    }
}
