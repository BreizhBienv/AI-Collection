using System;
using System.Collections.Generic;

public class FurnaceToRetrieve_Action : MoveToFurnace_Action
{
    public FurnaceToRetrieve_Action(Action pAction) : base(pAction)
    {
        _conditions.Add(EWorldState.AVAILABLE_INGOT, true);
    }
}
