using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreChunk : MonoBehaviour
{
    [SerializeField] private int _amount;
    public int Amount { get { return _amount; } }

    private bool _isOccupied = false;

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

    public void ReserveChunk(bool pIsReserved)
    {
        _isOccupied = pIsReserved;
    }

    public int PickUpOre(int pAmount = 1)
    {
        int numOreToPick = pAmount;

        if (pAmount > _amount)
            numOreToPick = _amount;

        _amount -= pAmount;

        if (_amount <= 0)
            Destroy(gameObject);

        return numOreToPick;
    }
}
