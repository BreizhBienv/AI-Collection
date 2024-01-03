using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreChunk : MonoBehaviour
{
    [SerializeField] private int _amount;
    public int Amount { get { return _amount; } }

    private bool _isOccupied = true;

    private void Start()
    {
        World.Instance.RegisterOre(this);
    }

    private void OnDestroy()
    {
        World.Instance.UnregisterOre(this);
    }

    public bool IsOccupied()
    {
        return _isOccupied;
    }

    public void ReserveChunk(bool _isReserved)
    {
        _isOccupied = _isReserved;
    }

    public int PickUpOre(int amount = 1)
    {
        int numOreToPick = amount;

        if (amount > _amount)
            numOreToPick = _amount;

        _amount -= amount;

        if (_amount <= 0)
            Destroy(gameObject);

        return numOreToPick;
    }
}
