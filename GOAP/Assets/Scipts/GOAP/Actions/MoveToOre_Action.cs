using System.Collections.Generic;
using UnityEngine;

public class MoveToOre_Action : BaseAction
{
    public MoveToOre_Action()
    {
        _conditions.Add(EWorldState.AVAILABLE_CHUNK,    true);
        _conditions.Add(EWorldState.NEAR_CHUNK,         false);
        _conditions.Add(EWorldState.HAS_ORES,           false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWS = new(pSimulated)
        {
            [EWorldState.NEAR_CHUNK]    = true,
            [EWorldState.NEAR_CHEST]    = false,
            [EWorldState.NEAR_FURNACE]  = false,
            [EWorldState.NEAR_PICKAXE]  = false,
        };

        return newWS;
    }

    public override void StartAction(MinerAgent pAgent)
    {
        pAgent._perceivedWorldState[EWorldState.NEAR_CHEST]     = false;
        pAgent._perceivedWorldState[EWorldState.NEAR_FURNACE]   = false;
        pAgent._perceivedWorldState[EWorldState.NEAR_PICKAXE]   = false;
    }

    public override void Execute(MinerAgent pAgent)
    {
        OreChunk chunk = pAgent._target?.GetComponent<OreChunk>();
        if (chunk != null && !chunk.IsOccupied())
            return;

        List<OreChunk> chunks = World.Instance.GetAvailableOreChunks();
        if (chunks.Count <= 0)
        {
            pAgent._target = null;
            return;
        }

        int rand = Random.Range(0, chunks.Count - 1);
        pAgent._target = chunks[rand].gameObject;
        pAgent._navMeshAgent.SetDestination(pAgent._target.transform.position);

        Debug.Log("Moving To " + pAgent._target.name);
    }

    public override bool IsComplete(MinerAgent pAgent, float pTimeInAction)
    {
        if (pAgent._target == null || !pAgent.CloseEnoughToTarget())
            return false;

        Debug.Log("Reached " + pAgent._target.name);
        return true;
    }

    public override void FinishAction(MinerAgent pAgent)
    {
        pAgent._perceivedWorldState[EWorldState.NEAR_CHUNK] = true;
    }

    public override void AbortAction(MinerAgent pAgent)
    {
    }
}
