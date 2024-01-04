using System;
using System.Collections.Generic;

public class MoveToChest_Action : BaseAction
{
    public MoveToChest_Action(Action pAction) : base(pAction)
    {
        _conditions.Add(EWorldState.NEAR_CHEST, false);
        _conditions.Add(EWorldState.HAS_INGOTS, true);
        _conditions.Add(EWorldState.INGOT_DELIVERED, false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWS = new(pSimulated)
        {
            [EWorldState.NEAR_CHEST] = true
        };

        return newWS;
    }
}
