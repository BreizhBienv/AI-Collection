using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineWithPickaxe_Action : MineOre_Action
{
    public MineWithPickaxe_Action()
    {
        _conditions.Add(EWorldState.HAS_PICKAXE, true);

        _actionDuration = 1;
        _cost = 1;
    }

    public override void StartAction(MinerAgent pAgent)
    {
        OreChunk chunk = pAgent._target?.GetComponent<OreChunk>();
        if (chunk == null)
        {
            pAgent._perceivedWorldState[EWorldState.NEAR_CHUNK] = false;
            pAgent._perceivedWorldState[EWorldState.HAS_PICKAXE] = false;
            pAgent.UnequipPickaxe();
            return;
        }

        chunk.ReserveChunk(pAgent);
    }

    public override void FinishAction(MinerAgent pAgent)
    {
        OreChunk chunk = pAgent._target?.GetComponent<OreChunk>();

        pAgent._orePossesed += chunk.PickUpOre();

        if (pAgent._orePossesed >= Utils.oreNeededToCraft)
        {
            chunk?.ReserveChunk(null);
            pAgent.UnequipPickaxe();
            pAgent._perceivedWorldState[EWorldState.HAS_ORES] = true;
        }

        if (chunk == null || chunk.Amount <= 0)
        {
            pAgent._target = null;
            pAgent._perceivedWorldState[EWorldState.NEAR_CHUNK] = false;
        }
    }
}
