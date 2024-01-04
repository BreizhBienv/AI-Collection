using System;
using System.Collections.Generic;

public class FurnaceToProcess_Action : MoveToFurnace_Action
{
    public FurnaceToProcess_Action(Action pAction) : base(pAction)
    {
        _conditions.Add(EWorldState.AVAILABLE_FURNACE,  true);
        _conditions.Add(EWorldState.HAS_ORES,           true);
    }
}
