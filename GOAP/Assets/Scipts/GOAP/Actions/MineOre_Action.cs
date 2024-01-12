using System.Collections.Generic;
using UnityEngine;

public class MineOre_Action : BaseAction
{
    public float _actionDuration = 2f;
    public MineOre_Action()
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

    public override void StartAction(MinerAgent pAgent)
    {
        OreChunk chunk = pAgent._target?.GetComponent<OreChunk>();
        if (chunk == null)
        {
            pAgent._perceivedWorldState[EWorldState.NEAR_CHUNK] = false;
            return;
        }

        chunk.ReserveChunk(pAgent);
    }

    public override void Execute(MinerAgent pAgent)
    {
        OreChunk chunk = pAgent._target?.GetComponent<OreChunk>();
        if (chunk == null)
            return;

        Debug.Log("Mining Ore");
    }

    public override bool IsComplete(MinerAgent pAgent, float pTimeInAction)
    {
        if (pTimeInAction < _actionDuration)
            return false;

        return true;
    }

    public override void FinishAction(MinerAgent pAgent)
    {
        OreChunk chunk = pAgent._target?.GetComponent<OreChunk>();

        pAgent._orePossesed += chunk.PickUpOre();

        if (pAgent._orePossesed >= Utils.oreNeededToCraft)
        {
            chunk?.ReserveChunk(null);
            pAgent._perceivedWorldState[EWorldState.HAS_ORES] = true;
        }

        if (chunk == null || chunk.Amount <= 0)
        {
            pAgent._target = null;
            pAgent._perceivedWorldState[EWorldState.NEAR_CHUNK] = false;
        }
    }
}
