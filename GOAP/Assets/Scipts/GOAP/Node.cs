using System.Collections.Generic;

public class Node
{
    public Node _parent = null;
    public Dictionary<EWorldState, bool> _worldState;
    public List<BaseAction> _actions = null;
    public int _totalCost = 0;

    public Node(Dictionary<EWorldState, bool> pWorldState)
    {
        _worldState = pWorldState;
    }

    public Node(Node pParent, Dictionary<EWorldState, bool> pWorldState, BaseAction pAction)
    {
        _parent = pParent;
        _worldState = pWorldState;
        _actions = pParent == null ? new() : new(pParent._actions)
        {
            pAction
        };

        int parentCost = pParent == null ? 0 : pParent._totalCost;
        _totalCost = parentCost + pAction._cost;
    }
}
