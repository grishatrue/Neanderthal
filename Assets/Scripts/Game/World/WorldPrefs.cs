using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldPrefs : MonoBehaviour
{
    private const int worldSide = 100;
    private static List<int> stonesArea = new List<int>()
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
    private static List<string> locations = new List<string>
    {
        "Mountain",
        "Forest",
        "Plain",
        "Coast",
        "Sea"
    };
    [SerializeField] private static List<LocationPattern> locationPatterns = new List<LocationPattern>(); // Mountain -> Sea
    [SerializeField] private static List<LocationPattern> specialPatterns;
    private static List<SpecialRoomCarcaseStruct> specialRoomsCarcases = new List<SpecialRoomCarcaseStruct>()
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
    
    public static List<int> StonesArea { get { return stonesArea; } }
    public static List<string> Locations { get { return locations; } }
    public static List<LocationPattern> LocationPatterns { get { return locationPatterns; } }
    public static List<LocationPattern> SpecialPatterns { get { return specialPatterns; } }
    public static List<SpecialRoomCarcaseStruct> SpecialRoomsCarcases { get { return specialRoomsCarcases; } }
    public static int WorldSide => worldSide;
}

public struct Tile
{
    public int tileIndex;
    public bool isWall;
    public List<int> bushesIndexes;
    public List<Vector2> bushesPositions;
    public List<bool> isBushBig;

    public Tile(int tileIndex, bool isWall, List<int> bushesIndexes, List<Vector2> bushesPositions, List<bool> isBushBig)
    {
        this.tileIndex = tileIndex;
        this.isWall = isWall;
        this.bushesIndexes = bushesIndexes;
        this.bushesPositions = bushesPositions;
        this.isBushBig = isBushBig;
    }
}

public struct SpecialRoomCarcaseStruct
{
    public string name;
    public int count;
    public string sourceLocation;
    public List<int> tilesIndexes;

    public SpecialRoomCarcaseStruct(string name, int count, string sourceLocation, List<int> tilesIndexes)
    {
        this.name = name;
        this.count = count;
        this.sourceLocation = sourceLocation;
        this.tilesIndexes = tilesIndexes;
    }
}
