using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveToOre_Action : BaseAction
{
    public MoveToOre_Action(Action pAction) 
        : base(pAction)
    {
        _conditions.Add(EWorldState.AVAILABLE_CHUNK,    true);
        _conditions.Add(EWorldState.NEAR_CHUNK,         false);
        _conditions.Add(EWorldState.HAS_ORES,           false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWS = new(pSimulated)
        {
            [EWorldState.NEAR_CHUNK] = true
        };

        return newWS;
    }

    public override void Execute(MinerAgent pAgent)
    {
        Debug.Log("Moved To Ore");

        if (pAgent._target == null)
        {
            List<OreChunk> chunks = World.Instance.GetAvailableOreChunks();
            int rand = UnityEngine.Random.Range(0, chunks.Count - 1);
            pAgent._target = chunks[rand].gameObject;
        }
        else
        {
            OreChunk chunk = pAgent._target.GetComponent<OreChunk>();
            if (!chunk.IsOccupied())
                return;

            List<OreChunk> chunks = World.Instance.GetAvailableOreChunks();
            int rand = UnityEngine.Random.Range(0, chunks.Count - 1);
            pAgent._target = chunks[rand].gameObject;

        }

        pAgent._navMeshAgent.SetDestination(pAgent._target.transform.position);
    }

    public override bool IsComplete(MinerAgent pAgent)
    {
        return pAgent.CloseEnoughToTarget();
    }
}
