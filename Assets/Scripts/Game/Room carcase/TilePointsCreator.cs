using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePointsCreator : MonoBehaviour
{
    public Camera cam;
    public int worldSide = WorldPrefs.WorldSide;
    private float tileSize;
    private List<Vector2> tilesPoints = new List<Vector2>();

    public float TileSize => tileSize;

    void Start()
    {
        float cameraSize = cam.orthographicSize * 2;
        tileSize = cameraSize / worldSide;
        var yPos = ((cameraSize / 2) - (tileSize / 2)) * 10;

        for (int y = 0; y < worldSide; y++)
        {
            var xPos = ((-cameraSize / 2) + (cameraSize / worldSide / 2)) * 10;

            for (int x = 0; x < worldSide; x++)
            {
                tilesPoints.Add(new Vector2(xPos, yPos));
                xPos += tileSize * 10;
            }

            yPos -= tileSize * 10;
        }
    }
}
