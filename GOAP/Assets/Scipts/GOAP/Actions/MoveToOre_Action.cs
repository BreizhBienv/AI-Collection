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

    public override void StartAction(MinerAgent pAgent)
    {
        if (_hasStarted)
            return;

        _hasStarted = true;

        pAgent._perceivedWorldState[EWorldState.NEAR_CHEST] = false;
        pAgent._perceivedWorldState[EWorldState.NEAR_FURNACE] = false;

        List<OreChunk> chunks = World.Instance.GetAvailableOreChunks();

        if (chunks.Count <= 0)
            return;

        int rand = UnityEngine.Random.Range(0, chunks.Count - 1);

        pAgent._target = chunks[rand].gameObject;
        Debug.Log("Moving To " + pAgent._target.name);

        pAgent._navMeshAgent.SetDestination(pAgent._target.transform.position);
    }

    public override void Execute(MinerAgent pAgent)
    {
        OreChunk chunk = pAgent._target?.GetComponent<OreChunk>();
        if (chunk != null && !chunk.IsOccupied())
            return;

        pAgent._target = null;
        pAgent._navMeshAgent.isStopped = true;
    }

    public override bool IsComplete(MinerAgent pAgent)
    {
        if (pAgent._target == null || !pAgent.CloseEnoughToTarget())
            return false;

        Debug.Log("Reached " + pAgent._target.name);
        return true;
    }

    public override void OnFinished(MinerAgent pAgent)
    {
        pAgent._perceivedWorldState[EWorldState.NEAR_CHUNK] = true;
    }

}
