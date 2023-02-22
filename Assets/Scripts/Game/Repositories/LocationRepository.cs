using System;
using System.Collections.Generic;
using UnityEngine;

public class LocationRepository : MonoBehaviour
{
    private readonly List<int> stonesArea = new List<int>()
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 1, 1, 1, 1, 1, 1, 0, 0,
            0, 1, 1, 1, 1, 1, 1, 1, 1, 0,
            0, 1, 1, 1, 1, 1, 1, 1, 1, 0,
            0, 1, 1, 1, 1, 1, 1, 1, 1, 0,
            0, 1, 1, 1, 1, 1, 1, 1, 1, 0,
            0, 1, 1, 1, 1, 1, 1, 1, 1, 0,
            0, 1, 1, 1, 1, 1, 1, 1, 1, 0,
            0, 0, 1, 1, 1, 1, 1, 1, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        }.FindAll(s => s == 1);
    public List<int> StonesArea { get => stonesArea; }


    private readonly List<string> locations = new List<string>
    {
        "Mountain",
        "Forest",
        "Plain",
        "Coast",
        "Sea"
    };
    public List<string> Locations { get => locations; }


    [SerializeField] private List<LocationPattern> locationPatterns = new List<LocationPattern>(); // Mountain -> Sea
    public List<LocationPattern> LocationPatterns { get { return locationPatterns; } set { locationPatterns = value; } }

    private readonly List<SpecialRoomCarcaseStruct> specialRoomsCarcases = new List<SpecialRoomCarcaseStruct>()
    {
        new SpecialRoomCarcaseStruct("Home village", 1, "Plain", new List<int>()
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        }),
        new SpecialRoomCarcaseStruct("Enemie village", 4, "Coast", new List<int>()
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        }),
        new SpecialRoomCarcaseStruct("Shaman\'s house", 2, "Forest", new List<int>()
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        }),
        new SpecialRoomCarcaseStruct("Dange", 4, "Plain", new List<int>()
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        }),
        new SpecialRoomCarcaseStruct("Volcano", 1, "Mountain", new List<int>()
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        }),
    };
    public List<SpecialRoomCarcaseStruct> SpecialRoomsCarcases { get => specialRoomsCarcases; }


    [SerializeField] private List<LocationPattern> specialPatterns;
    public List<LocationPattern> SpecialPatterns { get => specialPatterns; set => specialPatterns = value; }
}

[Serializable] public struct Tile
{
    public int TileIndex;
    public bool IsWall;
    public List<int> BushesIndexes;
    public List<Vector2Serializable> BushesPositions;
    public List<bool> IsBushBig;

    public Tile(int tileIndex, bool isWall, List<int> bushesIndexes, List<Vector2Serializable> bushesPositions, List<bool> isBushBig)
    {
        TileIndex = tileIndex;
        IsWall = isWall;
        BushesIndexes = bushesIndexes;
        BushesPositions = bushesPositions;
        IsBushBig = isBushBig;
    }
}

[Serializable] public struct Vector2Serializable
{
    public float x;
    public float y;

    public Vector2Serializable(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

public struct SpecialRoomCarcaseStruct
{
    public string Name;
    public int Count;
    public string SourceLocation;
    public List<int> TilesIndexes;

    public SpecialRoomCarcaseStruct(string name, int count, string sourceLocatio, List<int> tilesIndexes)
    {
        Name = name;
        Count = count;
        SourceLocation = sourceLocatio;
        TilesIndexes = tilesIndexes;
    }
}
