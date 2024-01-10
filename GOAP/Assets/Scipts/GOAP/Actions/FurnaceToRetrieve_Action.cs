using System;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceToRetrieve_Action : MoveToFurnace_Action
{
    public FurnaceToRetrieve_Action()
    {
        _conditions.Add(EWorldState.AVAILABLE_INGOT, true);
    }

    public override void Execute(MinerAgent pAgent)
    {
        Furnace furnace = pAgent._target?.GetComponent<Furnace>();
        if (furnace != null && furnace.CanPickUp())
            return;

        List<Furnace> furnaces = World.Instance.GetFurnacesWithIron();
        if (furnaces.Count <= 0)
            return;

        int rand = UnityEngine.Random.Range(0, furnaces.Count - 1);
        pAgent._target = furnaces[rand].gameObject;
        pAgent._navMeshAgent.SetDestination(pAgent._target.transform.position);

        Debug.Log("Moving To Furnace");
    }
}
