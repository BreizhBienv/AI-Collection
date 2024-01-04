using System;
using System.Collections.Generic;

public class MineOre_Action : BaseAction
{
    public MineOre_Action(Action pAction)
        : base(pAction)
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
}
