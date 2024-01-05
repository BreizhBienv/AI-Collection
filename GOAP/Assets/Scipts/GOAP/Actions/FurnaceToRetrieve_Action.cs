using System;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceToRetrieve_Action : MoveToFurnace_Action
{
    public FurnaceToRetrieve_Action(Action pAction) : base(pAction)
    {
        _conditions.Add(EWorldState.AVAILABLE_INGOT, true);
    }

    public override void Execute(MinerAgent pAgent)
    {
        List<Furnace> furnaces = World.Instance.GetFurnacesWithIron();
        int rand = UnityEngine.Random.Range(0, furnaces.Count - 1);

        if (pAgent._target == null)
        {
            Debug.Log("Moving To Furnace");
            pAgent._target = furnaces[rand].gameObject;
        }
        else
        {
            Furnace furnace = pAgent._target?.GetComponent<Furnace>();
            if (!furnace.CanPickUp())
                return;

            Debug.Log("Moving To Furnace");
            pAgent._target = furnaces[rand].gameObject;
        }

        base.Execute(pAgent);
    }
}
