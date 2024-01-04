using System;
using System.Collections.Generic;

public class MoveToFurnace_Action : BaseAction
{
    public MoveToFurnace_Action(Action pAction) : base(pAction)
    {
        _conditions.Add(EWorldState.NEAR_FURNACE, false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWorldState = new(pSimulated)
        {
            [EWorldState.NEAR_FURNACE] = true
        };

        return newWorldState;
    }
}
