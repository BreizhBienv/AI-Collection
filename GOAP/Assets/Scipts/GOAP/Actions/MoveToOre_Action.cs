using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToOre_Action : BaseAction
{
    public MoveToOre_Action()
    {
        _conditions.Add(EWorldState.AVAILABLE_CHUNK,    true);
        _conditions.Add(EWorldState.NEAR_CHUNK,         false);
        _conditions.Add(EWorldState.HAS_ORES,           false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWS = new Dictionary<EWorldState, bool>(pSimulated);
        newWS[EWorldState.NEAR_CHUNK] = true;

        return newWS;
    }

    public override void Execute()
    {
        Debug.Log("Move to Ore");
    }
}
