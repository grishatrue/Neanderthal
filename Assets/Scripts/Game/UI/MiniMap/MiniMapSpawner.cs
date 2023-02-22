using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapSpawner : MonoBehaviour
{
    [HideInInspector] public List<GameObject> MiniMapTiles;
    private GameObject newTile;
    public LocationRepository LocationRepository;

    public void SpawnMap()
    {
        RectTransform mapRectTransform = GetComponent<RectTransform>();

        float _width = (mapRectTransform.localPosition.x - mapRectTransform.anchorMin.x) * 2;
        float _height = (mapRectTransform.localPosition.y - mapRectTransform.anchorMin.y) * 2;

        float _x = mapRectTransform.localPosition.x - _width / 3;
        float _y = mapRectTransform.localPosition.y - _height / 3;

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                newTile = Instantiate(new GameObject(), Vector2.zero, Quaternion.identity, mapRectTransform);

                newTile.name = (3 * (y) + x + 1).ToString();
                newTile.GetComponent<Transform>().localPosition = new Vector2(_x, _y);
                newTile.GetComponent<Transform>().localScale = new Vector2(_x, _y);

                newTile.AddComponent<SpriteRenderer>().sprite = LocationRepository.LocationPatterns[0].Tiles[0];
                

                MiniMapTiles.Add(newTile);

                _x += mapRectTransform.rect.width / 3;
            }

            _x = mapRectTransform.localPosition.x - _width / 3; //
            _y += _height / 3;
        }
    }
}
