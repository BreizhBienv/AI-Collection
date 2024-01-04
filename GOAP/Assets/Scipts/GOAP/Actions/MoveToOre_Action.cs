using System;
using System.Collections.Generic;
public class MoveToOre_Action : BaseAction
{
    public MoveToOre_Action(Action pAction) 
        : base(pAction)
    {
        _conditions.Add(EWorldState.AVAILABLE_CHUNK,    true);
        _conditions.Add(EWorldState.NEAR_CHUNK,         false);
        _conditions.Add(EWorldState.HAS_ORES,           false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWS = new(pSimulated)
        {
            [EWorldState.NEAR_CHUNK] = true
        };

        return newWS;
    }
}
