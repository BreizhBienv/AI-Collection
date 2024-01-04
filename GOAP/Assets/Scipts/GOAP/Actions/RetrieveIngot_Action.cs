using System;
using System.Collections.Generic;

public class RetrieveIngot_Action : BaseAction
{
    public RetrieveIngot_Action(Action pAction) : base(pAction)
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
}
