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
        List<OreChunk> chunks = World.Instance.GetAvailableOreChunks();
        int rand = UnityEngine.Random.Range(0, chunks.Count - 1);

        if (pAgent._target == null)
        {
            pAgent._target = chunks[rand].gameObject;
            Debug.Log("Moving To " + pAgent._target.name);
        }
        else
        {
            OreChunk chunk = pAgent._target.GetComponent<OreChunk>();
            if (!chunk.IsOccupiedBy(pAgent))
                return;

            Debug.Log("Moving To " + pAgent._target.name);
            pAgent._target = chunks[rand].gameObject;
        }

        pAgent._navMeshAgent.SetDestination(pAgent._target.transform.position);
    }

    public override bool IsComplete(MinerAgent pAgent)
    {
        if (pAgent.CloseEnoughToTarget())
        {
            Debug.Log("Reached " + pAgent._target.name);
            return true;
        }

        return false;
    }
    public override void StartAction(MinerAgent pAgent)
    {
        base.StartAction(pAgent);

        pAgent._perceivedWorldState[EWorldState.NEAR_CHEST] = false;
        pAgent._perceivedWorldState[EWorldState.NEAR_FURNACE] = false;
    }

    public override void OnFinished(MinerAgent pAgent)
    {
        base.OnFinished(pAgent);

        pAgent._perceivedWorldState[EWorldState.NEAR_CHUNK] = true;
    }

}
