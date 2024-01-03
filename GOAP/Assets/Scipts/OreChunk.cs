using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreChunk : MonoBehaviour
{
    [SerializeField] private int _amount;
    public int Amount { get { return _amount; } }

    private int _reserved = 0;

    private void Start()
    {
        World.Instance.RegisterOre(this);
    }

    private void OnDestroy()
    {
        World.Instance.UnregisterOre(this);
    }


    public bool HasOreAvailable()
    {
        return _amount > 0;
    }

    public bool ReserveOre(int amount = 1)
    {
        if (_reserved >= amount)
            return true;

        if (HasOreAvailable())
        {
            _reserved += amount;
            _amount -= amount;
            return true;
        }
        return false;
    }

    public bool PickUpOre(int amount = 1)
    {
        if (amount > _reserved)
            return false;

        _reserved -= amount;

        if (_amount == 0 && _reserved == 0)
            Destroy(gameObject);

        return true;
    }
}
