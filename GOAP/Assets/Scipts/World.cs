using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class World : MonoBehaviour
{
    public static World Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        _worldState = new Dictionary<EWorldState, bool>()
        {
            { EWorldState.AVAILABLE_CHUNK,      false },
            { EWorldState.AVAILABLE_FURNACE,    false },
            { EWorldState.AVAILABLE_INGOT,      false },
        };
    }

    public List<OreChunk> _oreChunks { get; private set; } = new List<OreChunk>();
    public List<Furnace> _furnaces { get; private set; } = new List<Furnace>();
    public List<Chest> _chests { get; private set; } = new List<Chest>();

    [NonSerialized] public Dictionary<EWorldState, bool> _worldState;

    public void SetWorldState(EWorldState pState, bool pValue)
    {
        _worldState[pState] = pValue;
    }

    public bool GetWorldState(EWorldState pState)
    {
        return _worldState[pState];
    }

    public void RegisterOre(OreChunk ore)
    {
        if (!_oreChunks.Contains(ore))
        {
            _oreChunks.Add(ore);

            SetWorldState(EWorldState.AVAILABLE_CHUNK, true);
        }
    }

    public void UnregisterOre(OreChunk ore)
    {
        if (_oreChunks.Contains(ore))
            _oreChunks.Remove(ore);
    }

    public void RegisterFurnace(Furnace furnace)
    {
        if (!_furnaces.Contains(furnace))
        {
            _furnaces.Add(furnace);
            SetWorldState(EWorldState.AVAILABLE_FURNACE, true);
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

    public List<OreChunk> GetAvailableOreChunks()
    {
        return _oreChunks.Where(chunk => !chunk.IsOccupied()).ToList();
    }

    public List<Furnace> GetAvailableFurnaces(int oreAmount)
    {
        return _furnaces.Where(furnace => furnace.CanCraft(oreAmount)).ToList();
    }

    public List<Furnace> GetFurnacesWithIron()
    {
        return _furnaces.Where(furnace => furnace.CanPickUp()).ToList();
    }

    public Chest GetRandomChest()
    {
        int randChest = UnityEngine.Random.Range(0, _chests.Count - 1);
        return _chests[randChest];
    }
}
