using System;
using System.Collections.Generic;

public abstract class BaseAction
{
    protected Dictionary<EWorldState, bool> _conditions;

    public int  _cost = 1;
    protected Action _action;

    public BaseAction(Action pAction)
    {
        _conditions = new Dictionary<EWorldState, bool>();
        _action = pAction;
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

    public void Execute()
    {
        _action?.Invoke();
    }
}