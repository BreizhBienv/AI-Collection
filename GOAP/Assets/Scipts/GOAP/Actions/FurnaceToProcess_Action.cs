using System;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceToProcess_Action : MoveToFurnace_Action
{
    public FurnaceToProcess_Action()
    {
        _conditions.Add(EWorldState.AVAILABLE_FURNACE,  true);
        _conditions.Add(EWorldState.HAS_ORES,           true);
    }

    public override void Execute(MinerAgent pAgent)
    {
        Furnace furnace = pAgent._target?.GetComponent<Furnace>();
        if (furnace != null && furnace.CanCraft(pAgent._orePossesed))
            return;

        List<Furnace> furnaces = World.Instance.GetAvailableFurnaces(pAgent._orePossesed);
        if (furnaces.Count <= 0)
            return;

        int rand = UnityEngine.Random.Range(0, furnaces.Count - 1);
        pAgent._target = furnaces[rand].gameObject;
        pAgent._navMeshAgent.SetDestination(pAgent._target.transform.position);

        Debug.Log("Moving To Furnace");
    }
}
