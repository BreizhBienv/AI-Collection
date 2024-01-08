using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public abstract class BaseAction
{
    protected Dictionary<EWorldState, bool> _conditions;

    public int  _cost = 1;


    public BaseAction()
    {
        _conditions = new Dictionary<EWorldState, bool>();
    }

    public bool IsValid(Dictionary<EWorldState, bool> pSimulated)
    {
        foreach (var condition in _conditions)
        {
            pSimulated.TryGetValue(condition.Key, out bool value);
            if (condition.Value != value)
                return false;
        }

        return true;
    }

    public abstract Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated);


    public abstract void StartAction(MinerAgent pAgent);
    public abstract void Execute(MinerAgent pAgent);
    public abstract bool IsComplete(MinerAgent pAgent);
    public abstract void OnFinished(MinerAgent pAgent);
}