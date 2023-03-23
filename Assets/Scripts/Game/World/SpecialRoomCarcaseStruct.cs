using System.Collections.Generic;

public struct SpecialRoomCarcaseStruct
{
    public SpecialLocationEnum locName;
    public int roomsCount;
    public LocationEnum sourceLocation;
    public List<int> tilesIndexes;

    public SpecialRoomCarcaseStruct(SpecialLocationEnum locName, int roomsCount, LocationEnum sourceLocation, List<int> tilesIndexes)
    {
        this.locName = locName;
        this.roomsCount = roomsCount;
        this.sourceLocation = sourceLocation;
        this.tilesIndexes = tilesIndexes;
    }
}
