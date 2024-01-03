using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAP : MonoBehaviour
{
    [NonSerialized] public bool[] _goal;
    [NonSerialized] public bool[] _worldState;

    [NonSerialized] public Dictionary<Action, int> _actionPool;

    private void Awake()
    {
        
    }
}
