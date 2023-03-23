using System;
using System.Collections.Generic;
using UnityEngine;

public static class WorldAncillaryData
{
    private const int worldSide = 100;
    private static List<int> stonesArea = new List<int>()
        {
            1, 1, 1, 0, 0, 0, 0, 1, 1, 1,
            1, 1, 1, 1, 0, 0, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            0, 1, 1, 1, 1, 1, 1, 1, 1, 0,
            0, 0, 1, 1, 1, 1, 1, 1, 0, 0,
            0, 0, 1, 1, 1, 1, 1, 1, 0, 0,
            0, 1, 1, 1, 1, 1, 1, 1, 1, 0,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 0, 0, 1, 1, 1, 1,
            1, 1, 1, 0, 0, 0, 0, 1, 1, 1
        }.FindAll(s => s == 1);
    private static List<LocationResources> locationResources = new List<LocationResources>();
    private static List<LocationResources> specialResources = new List<LocationResources>();
    private static List<SpecialRoomCarcaseStruct> specialRoomsCarcases = new List<SpecialRoomCarcaseStruct>()
    {
        new SpecialRoomCarcaseStruct(SpecialLocationEnum.ENEMY_VILLAGE, 4, LocationEnum.COAST, new List<int>()
        {
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
        }),
        new SpecialRoomCarcaseStruct(SpecialLocationEnum.HOME_VILLAGE, 1, LocationEnum.PLAIN, new List<int>()
        {
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
        }),
        new SpecialRoomCarcaseStruct(SpecialLocationEnum.DANGE, 4, LocationEnum.PLAIN, new List<int>()
        {
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
        }),
        new SpecialRoomCarcaseStruct(SpecialLocationEnum.SHAMAN_HOUSE, 2, LocationEnum.FOREST, new List<int>()
        {
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
        }),
        new SpecialRoomCarcaseStruct(SpecialLocationEnum.VOLCANO, 1, LocationEnum.MOUNTAIN, new List<int>()
        {
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
        }),
    };
    
    public static List<int> StonesArea => stonesArea;
    //public static List<LocationResources> LocationResourcesList => locationResources; // replaced by method "GetLocationResources"
    //public static List<LocationResources> SpecialResources => specialResources; // replaced by method "GetSpecialLocationResources"
    public static List<SpecialRoomCarcaseStruct> SpecialRoomCarcases => specialRoomsCarcases;
    public static int WorldSide => worldSide;

    public static List<LocationResources> GetLocationResources()
    {
        if (locationResources.Count == 0) 
        {
            var tiles = Resources.LoadAll<Sprite>("World Generation/Tiles");
            var bushes = Resources.LoadAll<Sprite>("World Generation/Bushes");
            var localLocations = new List<string>();
            var res = new List<LocationResources>();

            foreach (string i in Enum.GetNames(typeof(LocationEnum)))
            {
                localLocations.Add(i.ToLower());
            }

            for (int i = 0; i < localLocations.Count; i++)
            {
                List<Sprite> tilesL = new List<Sprite>(); // L - located
                List<Sprite> wallsL = new List<Sprite>();
                List<Sprite> bushesL = new List<Sprite>();
                List<Sprite> bigBushesL = new List<Sprite>();

                for (int fileInd = 0; fileInd < tiles.Length; fileInd++) // tiles norm
                {
                    if (tiles[fileInd].name.Split('-')[0].ToLower() == localLocations[i])
                    {
                        tilesL.Add(tiles[fileInd]);
                    }
                }

                for (int fileInd = 0; fileInd < tiles.Length; fileInd++) // walls
                {
                    if (tiles[fileInd].name.Split('-')[1].ToLower() == "wall")
                    {
                        wallsL.Add(tiles[fileInd]);
                    }
                }

                for (int fileInd = 0; fileInd < bushes.Length; fileInd++) // bushes low
                {
                    if (bushes[fileInd].name.Split('-')[0].ToLower() == localLocations[i])
                    {
                        bushesL.Add(bushes[fileInd]);
                    }
                }

                for (int fileInd = 0; fileInd < bushes.Length; fileInd++) // bushes big
                {
                    if (bushes[fileInd].name.Split('-')[1].ToLower() == "big")
                    {
                        bigBushesL.Add(bushes[fileInd]);
                    }
                }

                string locName = Enum.GetName(typeof(LocationEnum), i);
                res.Add(new LocationResources(locName.ToString().ToLower(), tilesL, wallsL, bushesL, bigBushesL));
            }

            locationResources = res; 
        }

        return locationResources;
    }

    public static List<LocationResources> GetSpecialLocationResources()
    {
        if (specialResources == new List<LocationResources>()) 
        {
            var tiles = Resources.LoadAll("World Generation/Special/Tiles", typeof(Sprite)) as Sprite[];
            var bushes = Resources.LoadAll("World Generation/Special/Bushes", typeof(Sprite)) as Sprite[];
            var localLocations = new List<string>();
            var res = new List<LocationResources>();

            foreach (string i in Enum.GetNames(typeof(LocationEnum)))
            {
                localLocations.Add(i.ToLower());
            }

            for (int i = 0; i < localLocations.Count; i++)
            {
                List<Sprite> tilesL = new List<Sprite>();
                List<Sprite> wallsL = new List<Sprite>();
                List<Sprite> bushesL = new List<Sprite>();
                List<Sprite> bigBushesL = new List<Sprite>();

                for (int fileInd = 0; fileInd < tiles.Length; fileInd++) // tiles norm
                {
                    if (tiles[fileInd].name.Split('-')[0].ToLower() == localLocations[i])
                    {
                        tilesL.Add(tiles[fileInd]);
                    }
                }

                for (int fileInd = 0; fileInd < tiles.Length; fileInd++) // walls
                {
                    if (tiles[fileInd].name.Split('-')[1].ToLower() == "wall")
                    {
                        wallsL.Add(tiles[fileInd]);
                    }
                }

                for (int fileInd = 0; fileInd < bushes.Length; fileInd++) // bushes low
                {
                    if (bushes[fileInd].name.Split('-')[0].ToLower() == localLocations[i])
                    {
                        bushesL.Add(bushes[fileInd]);
                    }
                }

                for (int fileInd = 0; fileInd < bushes.Length; fileInd++) // bushes big
                {
                    if (bushes[fileInd].name.Split('-')[1].ToLower() == "big")
                    {
                        bigBushesL.Add(bushes[fileInd]);
                    }
                }

                string locName = Enum.GetName(typeof(LocationEnum), i);
                res.Add(new LocationResources(locName.ToString().ToLower(), tilesL, wallsL, bushesL, bigBushesL));
            }

            specialResources = res;
        }

        return specialResources;
    }
}

public struct Tile
{
    public int tileIndex;
    public bool isWall;
    public List<Bush> bushes;

    public Tile(int tileIndex, bool isWall)
    {
        this.tileIndex = tileIndex;
        this.isWall = isWall;
        this.bushes = new List<Bush>();
    }

    public Tile(int tileIndex, bool isWall, List<Bush> bushes)
    {
        this.tileIndex = tileIndex;
        this.isWall = isWall;
        this.bushes = bushes;
    }
}

public struct Bush
{
    public int index;
    public Vector2 position;
    public bool isBig;

    public Bush(int index, Vector2 position, bool isBig)
    {
        this.index = index;
        this.position = position;
        this.isBig = isBig;
    }
}

public enum SpecialLocationEnum
{
    ENEMY_VILLAGE, HOME_VILLAGE, DANGE, SHAMAN_HOUSE, VOLCANO
}