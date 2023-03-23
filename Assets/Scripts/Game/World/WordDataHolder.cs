using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WordDataHolder
{
    public static WorldData world = new WorldData();

    /*
    public static bool TryGetWorldData(out WorldData worldOut, bool clearOld)
    {
        worldOut = new WorldData();
        if (world == new WorldData()) { return false; }
        if (clearOld) { world = new WorldData(); }
        else { worldOut = world; }
        return true;
    }*/
}
