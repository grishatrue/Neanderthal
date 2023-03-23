using System.Collections.Generic;

public class WorldData
{
    private string worldName;
    private int startRoomIndex;
    private int[] locationIndexMap;
    private List<List<Tile>> tileIndexMap;

    public string WorldName => worldName;
    public int StartRoomIndex => startRoomIndex;
    public int[] LocationIndexMap => locationIndexMap;
    public List<List<Tile>> TileIndexMap => tileIndexMap;

    public WorldData() { }
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