
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
    NEAR_CHUNK,
    NEAR_FURNACE,
    NEAR_CHEST,
    HAS_ORES,
    HAS_INGOTS,
    PROCESS_ORE,
    STORE_INGOT,
}

public enum EGoal : uint
{
    PROCESS_ORE,
    STORE_INGOT,
}