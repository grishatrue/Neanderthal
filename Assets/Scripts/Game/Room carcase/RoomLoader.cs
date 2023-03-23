using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoader : MonoBehaviour
{
    public TileGridCreator tileGridObj;
    private WorldData currentWorld = WordDataHolder.world;
    private List<LocationResources> locationResources;

    public WorldData CurrentWorld => currentWorld;

    private void Awake()
    {
        locationResources = WorldAncillaryData.GetLocationResources();
    }

    private void Start()
    {
        // await System.Threading.Tasks.Task.Run(() => LoadRoom(currentWorld.StartRoomIndex));
        LoadRoom(currentWorld.StartRoomIndex);
    }

    private void LoadRoom(int roomIndex)
    {
        for (int ti = 0; ti < currentWorld.TileIndexMap[roomIndex].Count; ti++)
        {
            Tile currTlie = currentWorld.TileIndexMap[roomIndex][ti];

            GameObject newTileObj = Instantiate(new GameObject());

            newTileObj.name = currTlie.tileIndex.ToString();

            Sprite currTileSprite = locationResources[currTlie.tileIndex].TilesSprites[ti];
            newTileObj.AddComponent<SpriteRenderer>().sprite = currTileSprite;


        }
    }
}
