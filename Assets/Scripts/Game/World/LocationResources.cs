using System;
using UnityEngine;
using System.Collections.Generic;
public class LocationResources
{
    private string location;
    private List<Sprite> tiles;
    private List<Sprite> walls;
    private List<Sprite> bushes;
    private List<Sprite> bigBushes;

    public string LocationName => location;
    public List<Sprite> TilesSprites => tiles;
    public List<Sprite> Walls => walls;
    public List<Sprite> Bushes => bushes;
    public List<Sprite> BigBushes => bigBushes;

    public LocationResources() { }
    /// <summary>
    /// !!! "location" parameter must be <see cref="LocationEnum"/><c>.[value].ToString().ToLower()</c>
    /// </summary>
    public LocationResources(string location, List<Sprite> tiles, List<Sprite> walls, List<Sprite> bushes, List<Sprite> bigBushes)
    {
        this.location = location;
        this.tiles = tiles;
        this.walls = walls;
        this.bushes = bushes;
        this.bigBushes = bigBushes;
    }
}
