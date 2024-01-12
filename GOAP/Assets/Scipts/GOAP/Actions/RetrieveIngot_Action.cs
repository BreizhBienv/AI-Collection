using System;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveIngot_Action : BaseAction
{
    public RetrieveIngot_Action()
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

    public override void StartAction(MinerAgent pAgent)
    {
        Furnace furnace = pAgent._target?.GetComponent<Furnace>();
        if (furnace != null)
            return;

        pAgent._perceivedWorldState[EWorldState.NEAR_FURNACE] = false;
    }


    public override void Execute(MinerAgent pAgent)
    {
        Furnace furnace = pAgent._target?.GetComponent<Furnace>();
        if (furnace == null)
            return;

        if (furnace.TryPickUp())
        {
            Debug.Log("Retrieved Ingot");
            pAgent._ingotPossesed++;
        }
    }

    public override bool IsComplete(MinerAgent pAgent, float pTimeInAction)
    {
        return true;
    }

    public override void FinishAction(MinerAgent pAgent)
    {
        if (pAgent._ingotPossesed <= 0)
            return;

        pAgent._perceivedWorldState[EWorldState.HAS_INGOTS] = true;
        
        pAgent.UnequipPickaxe();
        pAgent._perceivedWorldState[EWorldState.HAS_ORES] = true;
    }
}
