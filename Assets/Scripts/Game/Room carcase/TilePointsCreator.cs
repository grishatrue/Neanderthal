using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePointsCreator : MonoBehaviour
{
    public Camera Camera;
    public WorldCreator WorldCreator;

    private List<Vector2> tilesPoints = new List<Vector2>();
    public List<Vector2> TilesPoints
    {
        get { return tilesPoints; }
        private set { tilesPoints = TilesPoints; }
    }

    private float tileSize;
    public float TileSize
    {
        get { return tileSize; }
        private set { tileSize = TileSize; }
    }

    void Start()
    {
        float cameraSize = Camera.orthographicSize * 2;
        TileSize = cameraSize / WorldCreator.WorldSide;

        float xPos;
        float yPos = ((cameraSize / 2) - (tileSize / 2)) * 10;

        for (int y = 0; y < WorldCreator.WorldSide; y++)
        {
            xPos = ((-cameraSize / 2) + (cameraSize / WorldCreator.WorldSide / 2)) * 10;

            for (int x = 0; x < WorldCreator.WorldSide; x++)
            {
                TilesPoints.Add(new Vector2(xPos, yPos));

                xPos += tileSize * 10;
            }

            yPos -= tileSize * 10;
        }
    }
}
