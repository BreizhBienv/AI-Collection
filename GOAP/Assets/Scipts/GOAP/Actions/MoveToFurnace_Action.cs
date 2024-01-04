using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveToFurnace_Action : BaseAction
{
    public MoveToFurnace_Action(Action pAction) : base(pAction)
    {
        _conditions.Add(EWorldState.NEAR_FURNACE, false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWorldState = new(pSimulated)
        {
            [EWorldState.NEAR_FURNACE] = true
        };

        return newWorldState;
    }

    public override void Execute(MinerAgent pAgent)
    {
        Debug.Log("Moved To Furnace");

        if (pAgent._target == null)
        {
            List<Furnace> furnaces = World.Instance.GetAvailableFurnaces(pAgent._orePossesed);
            int rand = UnityEngine.Random.Range(0, furnaces.Count - 1);
            pAgent._target = furnaces[rand].gameObject;
        }
        else
        {
            Furnace furnace = pAgent._target.GetComponent<Furnace>();
            if (furnace.CanCraft(pAgent._orePossesed))
                return;

            List<Furnace> furnaces = World.Instance.GetAvailableFurnaces(pAgent._orePossesed);
            int rand = UnityEngine.Random.Range(0, furnaces.Count - 1);
            pAgent._target = furnaces[rand].gameObject;
        }

        pAgent._navMeshAgent.SetDestination(pAgent._target.transform.position);
    }

    public override bool IsComplete(MinerAgent pAgent)
    {
        return pAgent.CloseEnoughToTarget();
    }
}
