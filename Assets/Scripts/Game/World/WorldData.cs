using System.Collections.Generic;

public class WorldData
{
    private string worldName;
    private int startRoomIndex;
    private int[] locationIndexMap;
    private List<List<Tile>> tileIndexMap;

    public string WorldName => worldName;
    public string StartRoomIndex => worldName;
    public string LocationIndexMap => worldName;
    public string TileIndexMap => worldName;

    public WorldData(int startRoomIndex, int[] locationIndexMap, List<List<Tile>> tileIndexMap)
    {
        this.worldName = "World" + "-" + System.DateTime.Now.ToString().Replace(" ", "-").Replace(".", "-").Replace(":", "-");
        this.startRoomIndex = startRoomIndex;
        this.locationIndexMap = locationIndexMap;
        this.tileIndexMap = tileIndexMap;
    }

    public WorldData(string worldName, int startRoomIndex, int[] locationIndexMap, List<List<Tile>> tileIndexMap)
    {
        this.worldName = worldName;
        this.startRoomIndex = startRoomIndex;
        this.locationIndexMap = locationIndexMap;
        this.tileIndexMap = tileIndexMap;
    }
}