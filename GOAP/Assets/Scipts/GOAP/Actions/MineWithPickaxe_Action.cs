
using UnityEngine;

public class MineWithPickaxe_Action : MineOre_Action
{
    public MineWithPickaxe_Action()
    {
        _conditions.Add(EWorldState.HAS_PICKAXE, true);

        _actionDuration = 1;
        _cost = 1;
    }

    public override void FinishAction(MinerAgent pAgent)
    {
        OreChunk chunk = pAgent._target?.GetComponent<OreChunk>();

        pAgent._orePossesed += chunk.PickUpOre();
        chunk?.ReserveChunk(null);

        Debug.Log("Mined Ore with pickaxe");

        if (pAgent._orePossesed >= Utils.oreNeededToCraft)
        {
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
