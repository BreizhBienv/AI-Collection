using System;
using System.Collections.Generic;
using UnityEngine;

public class ProcessOre_Action : BaseAction
{
    public ProcessOre_Action()
    {
        _conditions.Add(EWorldState.AVAILABLE_FURNACE, true);
        _conditions.Add(EWorldState.NEAR_FURNACE, true);
        _conditions.Add(EWorldState.HAS_ORES, true);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWorldState = new(pSimulated)
        {
            [EWorldState.PROCESS_ORE] = true,
        };

        return newWorldState;
    }

    public override void StartAction(MinerAgent pAgent)
    {
        Furnace furnace = pAgent._target?.GetComponent<Furnace>();
        if (furnace == null)
        {
            pAgent._perceivedWorldState[EWorldState.NEAR_FURNACE] = false;
            return;
        }

        Debug.Log("Processing Ore");
    }

    public override void Execute(MinerAgent pAgent)
    {
        Furnace furnace = pAgent._target?.GetComponent<Furnace>();
        if (furnace == null || !furnace.CanCraft(pAgent._orePossesed))
            return;

        pAgent._orePossesed = furnace.TryCraft(pAgent._orePossesed);
    }

    public override bool IsComplete(MinerAgent pAgent)
    {
        return true;
    }

    public override void OnFinished(MinerAgent pAgent)
    {
        if (pAgent._orePossesed % 2 <= 0 && pAgent._orePossesed > 0)
            return;

        pAgent._perceivedWorldState[EWorldState.HAS_ORES] = false;
    }
}
