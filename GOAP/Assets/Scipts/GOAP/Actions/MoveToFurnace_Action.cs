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

    public override void StartAction(MinerAgent pAgent)
    {
        if (_hasStarted)
            return;

        _hasStarted = true;

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

    public override void OnFinished(MinerAgent pAgent)
    {
        pAgent._perceivedWorldState[EWorldState.NEAR_FURNACE] = true;
    }
}
