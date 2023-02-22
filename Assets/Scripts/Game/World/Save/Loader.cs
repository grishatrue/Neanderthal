using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class Loader : MonoBehaviour
{
    private BinaryFormatter formatterBin;

    public TilePointsCreator TilePointsCreator;
    public LocationRepository LocationRepository;

    public GameObject Camera;
    public GameObject TilePrefab;
    public GameObject TilesParant;

    private WorldStorage currentWorld = new WorldStorage();
    public WorldStorage CurrentWorld { get => currentWorld; private set => currentWorld = value; }

    public void DeserialiseWorld(string savePath, string worldName)
    {
        formatterBin = new BinaryFormatter();
        LocationRepository = new LocationRepository();
        object savedData = null;

        FileStream file = File.Open(savePath + "/" + worldName, FileMode.Open);
        savedData = formatterBin.Deserialize(file);
        file.Close();

        CurrentWorld = (WorldStorage)savedData;
        Debug.Log(CurrentWorld.StartRoomIndex);
        Debug.Log("loading\n" + savePath + "/" + worldName);
    }    
    
    public void ReadWorld(string savePath, string worldName)
    {
        string[] file = File.ReadAllLines(savePath + "/" + worldName);


    }

    public void LoadRoom(int roomIndex)
    {
        List<Vector2> tilePoints = TilePointsCreator.TilesPoints;
        float tileSize = TilePointsCreator.TileSize;

        Debug.Log("до лога");
        Debug.Log(CurrentWorld.LocationMap[roomIndex] + "в этом"); // error here
        Debug.Log(CurrentWorld.LocationMap.FindIndex(s => s == CurrentWorld.LocationMap[roomIndex]) + "по этому");

        var lr = LocationRepository.LocationPatterns[CurrentWorld.LocationMap.FindIndex(s => s == CurrentWorld.LocationMap[roomIndex])];
        List<Sprite> tilesRep = lr.Tiles;
        List<Sprite> bushesRep = lr.Bushes;

        List<Tile> tilesRoom = CurrentWorld.Map[roomIndex];

        for (int i = 0; i < TilePointsCreator.TilesPoints.Count; i++)
        {
            GameObject newTile = Instantiate(TilePrefab, Vector2.zero, Quaternion.identity);


            Debug.Log("loading");


            newTile.name = "tile" + i;
            newTile.transform.SetParent(TilesParant.transform);
            newTile.transform.localScale = new Vector2(tileSize, tileSize);
            newTile.transform.localPosition = tilePoints[i];

            SpriteRenderer tileSpriteRenderer = newTile.GetComponent<SpriteRenderer>();
            tileSpriteRenderer.sortingOrder = -100;
            tileSpriteRenderer.sprite = tilesRep[tilesRoom[i].TileIndex]; // qq

            List<Tile> bushesRoom = CurrentWorld.Map[roomIndex];

            for (int e = 0; e < bushesRoom.Count; e++)
            {
                GameObject newBush = Instantiate(new GameObject(), Vector2.zero, Quaternion.identity);
                newBush.name = "bush" + e;
                newBush.transform.SetParent(newTile.transform);
                newBush.transform.localScale = new Vector2(tileSize, tileSize);
                newBush.transform.localPosition = new Vector2(tilesRoom[i].BushesPositions[e].x, tilesRoom[i].BushesPositions[e].y);

                if (tilesRoom[i].IsBushBig[e])
                {
                    newBush.AddComponent<Collider2D>().isTrigger = true;
                }
            }
        }
    }
}
