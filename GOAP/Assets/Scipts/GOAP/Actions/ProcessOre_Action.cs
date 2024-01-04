using System;
using System.Collections.Generic;

public class ProcessOre_Action : BaseAction
{
    public ProcessOre_Action(Action pAction) : base(pAction)
    {
        _conditions.Add(EWorldState.AVAILABLE_FURNACE, true);
        _conditions.Add(EWorldState.NEAR_FURNACE, true);
        _conditions.Add(EWorldState.HAS_ORES, true);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWorldState = new(pSimulated)
        {
            [EWorldState.AVAILABLE_INGOT] = true,
        };

        return newWorldState;
    }
}
