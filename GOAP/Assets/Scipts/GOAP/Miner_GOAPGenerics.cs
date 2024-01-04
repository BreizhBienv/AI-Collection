
public static class Utils
{
    public static float distanceToTarget = 5f;
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
    STORE_INGOT,
}