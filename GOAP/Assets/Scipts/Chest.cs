using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private void Start()
    {
        World.Instance.RegisterChest(this);
    }

    private void OnDestroy()
    {
        World.Instance.UnregisterChest(this);
    }

    public void DepositIron()
    {
    }
}
