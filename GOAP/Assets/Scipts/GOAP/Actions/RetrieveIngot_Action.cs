using System;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveIngot_Action : BaseAction
{
    public RetrieveIngot_Action(Action pAction) : base(pAction)
    {
        _conditions.Add(EWorldState.AVAILABLE_INGOT,    true);
        _conditions.Add(EWorldState.NEAR_FURNACE,       true);
        _conditions.Add(EWorldState.HAS_INGOTS,         false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWS = new(pSimulated)
        {
            [EWorldState.HAS_INGOTS] = true
        };

        return newWS;
    }

    public override void Execute(MinerAgent pAgent)
    {
        Debug.Log("Ingot retrieved");

        Furnace furnace = pAgent._target.GetComponent<Furnace>();
        if (furnace == null)
            return;

        if (furnace.TryPickUp())
            pAgent._ingotPossesed++;
    }

    public override bool IsComplete(MinerAgent pAgent)
    {
        return true;
    }

    public override void StartAction(MinerAgent pAgent)
    {
        base.StartAction(pAgent);
    }

    public override void OnFinished(MinerAgent pAgent)
    {
        base.OnFinished(pAgent);

        if (pAgent._ingotPossesed > 0)
        {
            pAgent._target = null;
            pAgent._perceivedWorldState[EWorldState.HAS_INGOTS] = true;
        }
    }
}
