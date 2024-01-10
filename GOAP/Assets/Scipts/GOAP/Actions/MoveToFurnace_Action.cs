using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveToFurnace_Action : BaseAction
{
    public MoveToFurnace_Action()
    {
        _conditions.Add(EWorldState.NEAR_FURNACE, false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWorldState = new(pSimulated)
        {
            [EWorldState.NEAR_FURNACE] = true,
            [EWorldState.NEAR_CHEST] = false,
            [EWorldState.NEAR_CHUNK] = false
        };

        return newWorldState;
    }

    public override void StartAction(MinerAgent pAgent)
    {
        pAgent._perceivedWorldState[EWorldState.NEAR_CHEST] = false;
        pAgent._perceivedWorldState[EWorldState.NEAR_CHUNK] = false;
    }

    public override void Execute(MinerAgent pAgent)
    {
    }

    public override bool IsComplete(MinerAgent pAgent)
    {
        if (pAgent._target == null || !pAgent.CloseEnoughToTarget())
            return false;
        
        Debug.Log("Moved To Furnace");
        return true;
    }

    public override void FinishAction(MinerAgent pAgent)
    {
        pAgent._perceivedWorldState[EWorldState.NEAR_FURNACE] = true;
    }
}
