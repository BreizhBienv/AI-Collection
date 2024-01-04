using System.Collections.Generic;

public class Miner_GOAP
{
    static public void BuildGraph(Node pParent,
        List<Node> pLeaves, 
        List<BaseAction> pAvailableActions,
        Dictionary<EWorldState, bool> goal)
    {
        foreach (BaseAction action in pAvailableActions)
        {
            if (!action.IsValid(pParent._worldState))
                continue;

            //string debug = "";
            //foreach (var state in pParent._worldState)
            //    debug += (state.Value.ToString() + " ");

            //string debugBis = "";
            //foreach (var a in pAvailableActions)
            //    debugBis += (a.ToString() + " ");

            //UnityEngine.Debug.Log(action.ToString());
            //UnityEngine.Debug.Log(debug);
            //UnityEngine.Debug.Log(debugBis);

            Dictionary<EWorldState, bool> newWS = action.ApplyEffect(pParent._worldState);
            Node node = new Node(pParent, newWS, action);

            if (!GoalAchieved(newWS, goal))
            {
                //used action is removed
                List<BaseAction> remainingActions = new(pAvailableActions);
                remainingActions.Remove(action);

                //recursive call on new node
                BuildGraph(node, pLeaves, remainingActions, goal);
            }
            else
                pLeaves.Add(node);
        }
    }
    
    static private bool GoalAchieved(Dictionary<EWorldState, bool> pWS, Dictionary<EWorldState, bool> goal)
    {
        foreach (var item in goal)
        {
            if (item.Value != pWS[item.Key])
                return false;
        }

        return true;
    }
}