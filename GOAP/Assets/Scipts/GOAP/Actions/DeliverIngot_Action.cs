using System;
using System.Collections.Generic;

public class DeliverIngot_Action : BaseAction
{
    public DeliverIngot_Action(Action pAction) : base(pAction)
    {
        _conditions.Add(EWorldState.NEAR_CHEST,         true);
        _conditions.Add(EWorldState.HAS_INGOTS,         true);
        _conditions.Add(EWorldState.INGOT_DELIVERED,    false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWS = new(pSimulated)
        {
            [EWorldState.INGOT_DELIVERED] = true
        };

        return newWS;
    }
}
