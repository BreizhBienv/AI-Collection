
using System.Collections.Generic;

public static class Utils
{
    public static float stoppingDistance = 2f;
    public static int   oreNeededToCraft = 2;
}

public enum EWorldState : uint
{
    AVAILABLE_CHUNK,
    AVAILABLE_FURNACE,
    AVAILABLE_INGOT,
    AVAILABLE_PICKAXE,
    NEAR_CHUNK,
    NEAR_FURNACE,
    NEAR_CHEST,
    NEAR_PICKAXE,
    HAS_ORES,
    HAS_INGOTS,
    HAS_PICKAXE,
    PROCESS_ORE,
    STORE_INGOT,
}