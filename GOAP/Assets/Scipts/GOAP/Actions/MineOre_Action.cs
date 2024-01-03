using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineOre_Action : BaseAction
{
    public MineOre_Action()
    {
        _conditions.Add(EWorldState.NEAR_CHUNK,         true);
        _conditions.Add(EWorldState.AVAILABLE_CHUNK,    true);
        _conditions.Add(EWorldState.HAS_ORES,           false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWorldState = new Dictionary<EWorldState, bool>(pSimulated);
        newWorldState[EWorldState.HAS_ORES] = true;

        return newWorldState;
    }

    public override void Execute()
    {
        Debug.Log("Mine Ore");
    }
}
