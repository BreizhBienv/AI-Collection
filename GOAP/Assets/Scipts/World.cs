using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static World Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public List<OreChunk> _oreChunks { get; private set; } = new List<OreChunk>();
    public List<Furnace> _furnaces { get; private set; } = new List<Furnace>();
    public List<Chest> _chests { get; private set; } = new List<Chest>();

    public void RegisterOre(OreChunk ore)
    {
        if (!_oreChunks.Contains(ore))
        {
            _oreChunks.Add(ore);
        }
    }

    public void UnregisterOre(OreChunk ore)
    {
        if (_oreChunks.Contains(ore))
        {
            _oreChunks.Remove(ore);
        }
    }

    public void RegisterFurnace(Furnace furnace)
    {
        if (!_furnaces.Contains(furnace))
        {
            _furnaces.Add(furnace);
        }
    }

    public void UnregisterFurnace(Furnace furnace)
    {
        if (_furnaces.Contains(furnace))
        {
            _furnaces.Remove(furnace);
        }
    }

    public void RegisterChest(Chest chest)
    {
        if (!_chests.Contains(chest))
        {
            _chests.Add(chest);
        }
    }

    public void UnregisterChest(Chest chest)
    {
        if (_chests.Contains(chest))
        {
            _chests.Remove(chest);
        }
    }

    public List<Furnace> GetAvailableFurnaces(int oreAmount)
    {
        List<Furnace> availableFurnaces = new List<Furnace>();
        foreach (Furnace furnace in _furnaces)
        {
            if (furnace.CanCraft(oreAmount))
                availableFurnaces.Add(furnace);
        }
        return availableFurnaces;
    }

    public List<Furnace> GetFurnacesWithIron()
    {
        List<Furnace> availableFurnaces = new List<Furnace>();
        foreach (Furnace furnace in _furnaces)
        {
            if (furnace.CanPickUp())
                availableFurnaces.Add(furnace);
        }
        return availableFurnaces;
    }
}
