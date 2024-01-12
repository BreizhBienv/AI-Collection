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
            { EWorldState.AVAILABLE_CHUNK,      true },
            { EWorldState.AVAILABLE_FURNACE,    true },
            { EWorldState.AVAILABLE_INGOT,      false },
            { EWorldState.AVAILABLE_PICKAXE,    false },
        };
    }

    public List<OreChunk> _oreChunks { get; private set; } = new List<OreChunk>();
    public List<Furnace> _furnaces { get; private set; } = new List<Furnace>();
    public List<Chest> _chests { get; private set; } = new List<Chest>();
    public List<Pickaxe> _pickaxes { get; private set; } = new List<Pickaxe>();

    [NonSerialized] public Dictionary<EWorldState, bool> _worldState;

    public void UpdateWorldState_AvailableIngot()
    {
        int count = GetFurnacesWithIron().Count();
        if (count > 0)
            _worldState[EWorldState.AVAILABLE_INGOT] = true;
        else
            _worldState[EWorldState.AVAILABLE_INGOT] = false;
    }

    public void UpdateWorldState_AvailableFurnace()
    {
        int count = GetAvailableFurnaces(Utils.oreNeededToCraft).Count();
        if (count > 0)
            _worldState[EWorldState.AVAILABLE_FURNACE] = true;
        else
            _worldState[EWorldState.AVAILABLE_FURNACE] = false;
    }

    public void RegisterOre(OreChunk ore)
    {
        if (!_oreChunks.Contains(ore))
            _oreChunks.Add(ore);
    }

    public void UnregisterOre(OreChunk ore)
    {
        if (_oreChunks.Contains(ore))
            _oreChunks.Remove(ore);

        if (_oreChunks.Count <= 0)
            _worldState[EWorldState.AVAILABLE_CHUNK] = false;
    }

    public void RegisterFurnace(Furnace furnace)
    {
        if (!_furnaces.Contains(furnace))
            _furnaces.Add(furnace);
    }

    public void UnregisterFurnace(Furnace furnace)
    {
        if (_furnaces.Contains(furnace))
            _furnaces.Remove(furnace);
    }

    public void RegisterChest(Chest chest)
    {
        if (!_chests.Contains(chest))
            _chests.Add(chest);
    }

    public void UnregisterChest(Chest chest)
    {
        if (_chests.Contains(chest))
            _chests.Remove(chest);
    }

    public void RegisterPickaxe(Pickaxe pPickaxe)
    {
        if (_pickaxes.Contains(pPickaxe))
            return;

        _pickaxes.Add(pPickaxe);
        _worldState[EWorldState.AVAILABLE_PICKAXE] = true;
    }

    public void UnregisterPickaxe(Pickaxe pPickaxe)
    {
        if (!_pickaxes.Contains(pPickaxe))
            return;

        _pickaxes.Remove(pPickaxe);
        
        if (!_pickaxes.Any())
            _worldState[EWorldState.AVAILABLE_PICKAXE] = false;
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

    public Pickaxe GetRandomPickaxe()
    {
        int randChest = UnityEngine.Random.Range(0, _pickaxes.Count - 1);
        return _pickaxes[randChest];
    }

    public bool IsPickaxeAvailable(Pickaxe pPickaxe)
    {
        return _pickaxes.Contains(pPickaxe);
    }
}
