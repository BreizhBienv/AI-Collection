using System;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceToProcess_Action : MoveToFurnace_Action
{
    public FurnaceToProcess_Action(Action pAction) : base(pAction)
    {
        _conditions.Add(EWorldState.AVAILABLE_FURNACE,  true);
        _conditions.Add(EWorldState.HAS_ORES,           true);
    }

    public override void Execute(MinerAgent pAgent)
    {
        List<Furnace> furnaces = World.Instance.GetAvailableFurnaces(pAgent._orePossesed);
        int rand = UnityEngine.Random.Range(0, furnaces.Count - 1);

        if (pAgent._target == null)
        {
            Debug.Log("Moving To Furnace");
            pAgent._target = furnaces[rand].gameObject;
        }
        else
        {
            Furnace furnace = pAgent._target.GetComponent<Furnace>();
            if (furnace.CanCraft(pAgent._orePossesed))
                return;

            Debug.Log("Moving To Furnace");
            pAgent._target = furnaces[rand].gameObject;
        }

        base.Execute(pAgent);
    }
}
