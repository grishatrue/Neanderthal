﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WorldDataGenerator
{
    public WorldData GetNewWorldData()
    {
        LoadAndAllocateResouces();

        int startRoomIndex = 3400;
        int worldSide = WorldAncillaryData.WorldSide;
        int[] locationMap;
        List<int> locationsInt = new List<int>();
        int lastLocationIndex = (int)Location.SEA;

        for (int i = 0; i <= lastLocationIndex; i++)
        {
            locationsInt.Add(i);
        }

        locationMap = new int[worldSide * worldSide];

        for (int i = 0; i < worldSide * worldSide; i++)
        {
            locationMap[i] = locationsInt[locationsInt.Count - 1];
        }

        List<Vector2> points3 = new List<Vector2>();
        List<Vector2> points6 = new List<Vector2>();
        List<Vector2> points12 = new List<Vector2>();
        List<Vector2> points24 = new List<Vector2>();

        List<List<Vector2>> points = new List<List<Vector2>>() { points3, points6, points12, points24 };

        List<Vector2> possiblePoints = new List<Vector2>();

        int spaceBetweenPoints = 5;

        while (possiblePoints.Count == 0)
        {
            possiblePoints = new List<Vector2>();

            for (int y = 45; y <= 55; y++)
                for (int x = 45; x <= 55; x++)
                    if (x < 45 + 2 || x > 55 - 2 || y < 45 + 2 || y > 55 - 2)
                        possiblePoints.Add(new Vector2(x, y));
        }

        for (int i = 0; i < 3; i++)
        {
            Vector2 rp = possiblePoints[UnityEngine.Random.Range(0, possiblePoints.Count - 1)];
            points[0].Add(rp);

            for (int y = (int)rp[1] - spaceBetweenPoints; y < rp[1] + spaceBetweenPoints + 1; y++)
                for (int x = (int)rp[0] - spaceBetweenPoints; x < rp[0] + spaceBetweenPoints + 1; x++)
                    if (possiblePoints.Contains(new Vector2(x, y)))
                        possiblePoints.Remove(new Vector2(x, y));
        }

        int centerX = (int)((points3[0].x + points3[1].x + points3[2].x) / 3);
        int centerY = (int)((points3[0].y + points3[1].y + points3[2].y) / 3);

        // Получение контуров секторов
        for (int i = 0; i < 3; i++)
        {
            points[i + 1] = GetNewApexes(points[i], centerX, centerY);
        }

        // Заполнение секторов
        points.Reverse();
        for (int i = 0; i < points.Count; i++)
        {
            for (int e = 0; e < points[i].Count - 1; e++)
            {
                int rand;

                if (i == 1)
                    rand = locationsInt[locationsInt.Count - UnityEngine.Random.Range(1, 2) - 2];
                else
                    rand = locationsInt[locationsInt.Count - i - 2];

                FillInLines(new Vector2(centerX, centerY), points[i][e], points[i][e + 1], rand, worldSide, locationMap);
            }

            FillInLines(new Vector2(centerX, centerY), points[i][points[i].Count - 1], points[i][0], locationsInt[locationsInt.Count - i - 2], worldSide, locationMap);
        }
        points.Reverse();

        // Перенос точек на карту
        for (int location = 0; location < points.Count; location++)
        {
            for (int i = 0; i < points[location].Count; i++)
            {
                int _X = (int)points[location][i].x;
                int _Y = (int)points[location][i].y;
                locationMap[worldSide * (_Y - 1) + _X] = location;
            }
        }

        for (int i = 0; i < locationMap.Length; i++) // qq типа пофиксил "err01"
        {
            if (locationMap[i] == 9)
            {
                locationMap[i] = locationMap[i - 1];
            }
        }


        return new WorldData(startRoomIndex, locationMap, GenerateRooms(points, locationMap, worldSide, startRoomIndex));
    }

    private List<List<Tile>> GenerateRooms(List<List<Vector2>> pointMap, int[] locationMap, int worldSide, int startRoomIndex)
    {
        var localTileMap = new List<List<Tile>>();

        for (int i = 0; i < locationMap.Length; i++)
        {
            localTileMap.Add(GetNewRoomData(locationMap[i], worldSide)); // qq (err01) тут в LocationsMap[i] проскакивает 9 (символ предзаполнения)
        }

        for (int i = 0; i < WorldAncillaryData.SpecialRoomsCarcases.Count; i++)
        {
            int currentLocInd = (int)WorldAncillaryData.SpecialRoomsCarcases[i].sourceLocation;
            Vector2 currentLocPoints = pointMap[currentLocInd][UnityEngine.Random.Range(0, pointMap[currentLocInd].Count)];
            int randomRoomIndex = (int)(worldSide * (currentLocPoints.y - 1) + currentLocPoints.x);
            //localMap[randomRoomIndex] = GetSpecialRoomDataAtName(loc, localMap[randomRoomIndex]);
            localTileMap[randomRoomIndex] = GetSpecialRoomDataAtIndex(i);

            if (WorldAncillaryData.SpecialRoomsCarcases[i].name == "Home village")
            {
                startRoomIndex = randomRoomIndex;
            }
        }

        return localTileMap;
    }

    private List<Vector2> GetNewApexes(List<Vector2> p, int centerX, int centerY)
    {
        int n = p.Count;

        // np - New Points
        List<Vector2> np = new List<Vector2>(); // Массив точек контура новой локации
        List<Vector2> np1 = new List<Vector2>(); // Массив координат отрезков, идущих к вершине
        List<Vector2> np2 = new List<Vector2>(); // Массив координат отрезков, идущих к стороне

        // Коэффициенты увеличения удаленности вершин новых локаций от центра острова.
        float k1 = 0; // К вершине
        float k2 = 0; // К стороне

        int x;
        int y;

        if (n == 3)
        {
            k1 = 0.3f;
            k2 = 0.2f;
        }
        else if (n == 6)
        {
            k1 = 0.6f;
            k2 = 0.5f;
        }
        else if (n == 12)
        {
            k1 = 0.75f;
            k2 = 0.7f;
        }

        for (int i = 0; i < n; i++) // К вершине
        {
            x = (int)((p[i].x - centerX) / k1) + centerX;
            y = (int)((p[i].y - centerY) / k1) + centerY;
            np1.Add(new Vector2(x, y));
        }

        for (int i = 0; i < n - 1; i++) // К стороне
        {
            x = (int)(((int)((p[i].x + p[i + 1].x) / 2) - centerX) / k2) + centerX;
            y = (int)(((int)((p[i].y + p[i + 1].y) / 2) - centerY) / k2) + centerY;
            np2.Add(new Vector2(x, y));
        }
        // Последний отрезок к стороне
        x = (int)(((int)((p[n - 1].x + p[0].x) / 2) - centerX) / k2) + centerX;
        y = (int)(((int)((p[n - 1].y + p[0].y) / 2) - centerY) / k2) + centerY;
        np2.Add(new Vector2(x, y));

        // Сборка выходного массива
        for (int i = 0; i < n; i++)
        {
            np.Add(np1[i]);
            np.Add(np2[i]);
        }

        return np;
    }

    private void FillInLines(Vector2 center, Vector2 p0, Vector2 p1, int fill, int worldSide, int[] locationMap)
    {
        int extraFill = 9; // Символ предзаполнения
        List<Vector2> _tri = new List<Vector2> { center, p0, p1 }; // Сектор предзаполнения

        for (int i = 0; i < 3; i++)
        {
            int n = 0;
            float h = _tri[i].y - _tri[_tri.Count - i - 1].y;
            float w = _tri[i].x - _tri[_tri.Count - i - 1].x;
            float d = 0; // Longer vector
            float z = 0; // Shorter vector

            if (Mathf.Abs(w) > Mathf.Abs(h))
            {
                n = (int)w;
                d = w / Mathf.Abs(w);

                if (h == 0)
                    z = 0;
                else
                    z = Mathf.Abs(h / w) * (h / Mathf.Abs(h));
            }
            else
            {
                n = (int)h;
                d = h / Mathf.Abs(h);

                if (w == 0)
                    z = 0;
                else
                    z = Mathf.Abs(w / h) * (w / Mathf.Abs(w));
            }

            float nx = _tri[_tri.Count - i - 1].x;
            float ny = _tri[_tri.Count - i - 1].y;

            for (int j = 1; j < Mathf.Abs(n) + 1; j++) // Предзаполнение
            {
                int _ind = worldSide * ((int)Mathf.Round(ny) - 1) + (int)Mathf.Round(nx);

                locationMap[_ind] = extraFill;

                nx += Mathf.Abs(w) > Mathf.Abs(h) ? d : z;
                ny += Mathf.Abs(w) > Mathf.Abs(h) ? z : d;
            }
        }

        for (int y = 1; y < worldSide + 1; y++) // Заполнение
        {
            List<int> w = new List<int>();

            for (int i = worldSide * (y - 1); i < worldSide * y; y++)
            {
                w.Add(locationMap[i]);
            }

            if (w.Contains(extraFill))
            {
                List<int> foundPoints = new List<int>();

                for (int x = 0; x < w.Count; x++)
                {
                    if (w[x] == extraFill)
                    {
                        foundPoints.Add(worldSide * (y - 1) + x);
                    }
                }

                for (int x = foundPoints[0]; x < foundPoints[foundPoints.Count - 1] + 1; x++)
                {
                    locationMap[x] = fill;
                }
            }
        }
    }

    private List<Tile> GetNewRoomData(int locationIndex, int worldSide)
    {
        List<Tile> tiles = new List<Tile>();
        // загрузка ресов // qq // err
        LocationResources locRepos = WorldAncillaryData.LocationResourcesList[locationIndex];
        List<Sprite> tilesRep = locRepos.Tiles;
        List<Sprite> bushesRep = locRepos.Bushes;
        List<Sprite> wallsRep = locRepos.Walls;
        List<Sprite> bigBushesRep = locRepos.BigBushes;

        int wallsCount = UnityEngine.Random.Range(0, 5);

        List<int> stonesArea = WorldAncillaryData.StonesArea;
        List<int> stonesIndexes = new List<int>();

        for (int i = 0; i < wallsCount; i++)
        {
            int rand = UnityEngine.Random.Range(0, stonesArea.Count);
            stonesIndexes.Add(stonesArea[rand]);
            stonesArea.Remove(rand);
        }

        for (int i = 0; i < worldSide; i++)
        {
            int tileIndex;
            bool isWall;
            List<int> bushesIndexes = new List<int>();
            List<Vector2> bushesPositions = new List<Vector2>();
            List<bool> isBushBig = new List<bool>();

            if (stonesIndexes.Contains(i))
            {
                tileIndex = UnityEngine.Random.Range(0, wallsRep.Count);
                isWall = true;
            }
            else
            {
                tileIndex = UnityEngine.Random.Range(0, tilesRep.Count);
                isWall = false;
            }

            for (int e = 0; e < UnityEngine.Random.Range(0, bushesRep.Count); e++)
            {
                if (UnityEngine.Random.Range(0, 10) <= 3)
                {
                    bushesIndexes.Add(UnityEngine.Random.Range(0, bigBushesRep.Count));
                    isBushBig.Add(true);
                }
                else
                {
                    bushesIndexes.Add(UnityEngine.Random.Range(0, bushesRep.Count));
                    isBushBig.Add(false);
                }
                bushesPositions.Add(new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f)));
            }

            tiles.Add(new Tile(tileIndex, isWall, bushesIndexes, bushesPositions, isBushBig));
        }

        return tiles;
    }
    
    private List<Tile> GetSpecialRoomDataAtIndex(int index)
    {
        SpecialRoomCarcaseStruct newSpecialRoom = WorldAncillaryData.SpecialRoomsCarcases[index]; // допилить // qq

        return default;
    }

    public void LoadAndAllocateResouces()
    {
        var tiles = Resources.LoadAll<Sprite>("Game Objects/Tiles") as Sprite[];
        var bushes = Resources.LoadAll<Sprite>("Game Objects/Bushes") as Sprite[];
        var localLoc = new List<string>();

        foreach (string i in Enum.GetNames(typeof(Location)))
        {
            localLoc.Add(i.ToLower());
        }

        for (int i = 0; i < localLoc.Count; i++)
        {
            List<Sprite> tilesL = new List<Sprite>();
            List<Sprite> wallsL = new List<Sprite>();
            List<Sprite> bushesL = new List<Sprite>();
            List<Sprite> bigBushesL = new List<Sprite>();

            for (int fileInd = 0; fileInd < tiles.Length; fileInd++)
            {
                if (tiles[fileInd].name.Split('-')[0].ToLower() == localLoc[i]) // tiles norm
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
                if (bushes[fileInd].name.Split('-')[0].ToLower() == localLoc[i])
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

            string locName = Enum.GetName(typeof(Location), i);
            WorldAncillaryData.LocationResourcesList.Add(new LocationResources(locName.ToString().ToLower(), tilesL, wallsL, bushesL, bigBushesL));
        }
    }
}
