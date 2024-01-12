using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineWithHands_Action : MineOre_Action
{
    public MineWithHands_Action()
    {
        _conditions.Add(EWorldState.HAS_PICKAXE, false);

        _actionDuration = 2;
        _cost = 5;
    }
}
