using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipPickaxe_Action : BaseAction
{
    public EquipPickaxe_Action()
    {
        _conditions.Add(EWorldState.AVAILABLE_PICKAXE,  true);
        _conditions.Add(EWorldState.NEAR_PICKAXE,       true);
        _conditions.Add(EWorldState.HAS_PICKAXE,        false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWorldState = new(pSimulated)
        {
            [EWorldState.HAS_PICKAXE] = true,
        };

        return newWorldState;
    }

    public override void StartAction(MinerAgent pAgent)
    {
        Pickaxe pickaxe = pAgent._target?.GetComponent<Pickaxe>();
        if (pickaxe == null)
        {
            pAgent._perceivedWorldState[EWorldState.NEAR_PICKAXE] = false;
            return;
        }

        Debug.Log("Equiping pickaxe");
    }

    public override void Execute(MinerAgent pAgent)
    {
        Pickaxe pickaxe = pAgent._target?.GetComponent<Pickaxe>();
        if (pickaxe == null)
            return;

        pAgent.EquipPickaxe(pickaxe);
    }

    public override bool IsComplete(MinerAgent pAgent, float pTimeInAction)
    {
        return true;
    }

    public override void FinishAction(MinerAgent pAgent)
    {
        pAgent._perceivedWorldState[EWorldState.HAS_PICKAXE] = true;
    }
}
