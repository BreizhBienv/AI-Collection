using System;
using System.Collections.Generic;
using UnityEngine;

public class MineOre_Action : BaseAction
{
    public MineOre_Action(Action pAction) : base(pAction)
    {
        _conditions.Add(EWorldState.AVAILABLE_CHUNK,    true);
        _conditions.Add(EWorldState.NEAR_CHUNK,         true);
        _conditions.Add(EWorldState.HAS_ORES,           false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWorldState = new(pSimulated)
        {
            [EWorldState.HAS_ORES] = true
        };

        return newWorldState;
    }

    public override void Execute(MinerAgent pAgent)
    {
        Debug.Log("Mined Ore");

        OreChunk chunk = pAgent._target.GetComponent<OreChunk>();
        if (chunk == null)
            return;

        pAgent._orePossesed += chunk.PickUpOre();
    }

    public override bool IsComplete(MinerAgent pAgent)
    {
        return true;
    }

    public override void StartAction(MinerAgent pAgent)
    {
        base.StartAction(pAgent);

        pAgent._target?.GetComponent<OreChunk>()?.ReserveChunk(pAgent);
    }

    public override void OnFinished(MinerAgent pAgent)
    {
        base.OnFinished(pAgent);

        if (pAgent._orePossesed >= Utils.oreNeededToCraft)
        {
            pAgent._target.GetComponent<OreChunk>()?.ReserveChunk(null);
            pAgent._target = null;
            pAgent._perceivedWorldState[EWorldState.HAS_ORES] = true;
        }
    }
}
