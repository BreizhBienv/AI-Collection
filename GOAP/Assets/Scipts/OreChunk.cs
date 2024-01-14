using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreChunk : MonoBehaviour
{
    [SerializeField] private int _amount;
    public int Amount { get { return _amount; } }

    private MinerAgent _occupiedBy = null;

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
        return _occupiedBy != null;
    }

    public void ReserveChunk(MinerAgent pReservedBy)
    {
        _occupiedBy = pReservedBy;
        //World.Instance.UpdateAvailableChunk();
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
