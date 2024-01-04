using System;
using System.Collections.Generic;
using UnityEngine;

public class ProcessOre_Action : BaseAction
{
    public ProcessOre_Action(Action pAction) : base(pAction)
    {
        _conditions.Add(EWorldState.AVAILABLE_FURNACE, true);
        _conditions.Add(EWorldState.NEAR_FURNACE, true);
        _conditions.Add(EWorldState.HAS_ORES, true);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWorldState = new(pSimulated)
        {
            [EWorldState.AVAILABLE_INGOT] = true,
        };

        return newWorldState;
    }

    public override void Execute(MinerAgent pAgent)
    {
        Debug.Log("Processed Ore");

        Furnace furnace = pAgent._target.GetComponent<Furnace>();
        if (furnace == null || !furnace.CanCraft(pAgent._orePossesed))
            return;

        pAgent._orePossesed = furnace.TryCraft(pAgent._orePossesed);
    }

    public override bool IsComplete(MinerAgent pAgent)
    {
        return true;
    }
}
