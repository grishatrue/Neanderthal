using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class WorldCreator : MonoBehaviour
{
    [Tooltip("Неизменяемое поле")] private int worldSide = 100;
    public int WorldSide { get => worldSide; }

    private int[] locationsMap;
    public int[] LocationsMap { get => locationsMap; private set => locationsMap = value; }

    public List<List<Tile>> map = new List<List<Tile>>();

    private bool isWorldMapFull = false;
    public bool IsWorldMapFull { get => isWorldMapFull; set => isWorldMapFull = value; }

    public LocationRepository LocationRepository;
    private Saver saver;

    private int startRoomIndex = 3400;
    public int StartRoomIndex { get => startRoomIndex; set => startRoomIndex = value; }

    public List<string> locationsStr;
    private readonly List<int> locationsInt = new List<int>();


    IEnumerator RoomCreationBGProc(List<List<Vector2>> p)
    {
        int numToPass = 300;

        for (int i = 0; i < LocationsMap.Length; i++)
        {
            map.Add(CreateRoomData(LocationsMap[i])); // qq (err01) тут в LocationsMap[i] проскакивает 9 (символ предзаполнения)

            if (i % numToPass == 0)
            {
                yield return new WaitForSeconds(0.1f);
                Debug.Log(i + " rooms created"); // прогресбар внизу страницы
            }
        }

        for (int i = 0; i < LocationRepository.SpecialRoomsCarcases.Count; i++)
        {
            int loc = locationsStr.FindIndex(s => s == LocationRepository.SpecialRoomsCarcases[i].SourceLocation);
            Vector2 _ = p[loc][UnityEngine.Random.Range(0, p[loc].Count)];
            int randomRoomIndex = (int)(100 * (_.y - 1) + _.x);
            map[randomRoomIndex] = ChangeRoomToSpecial(loc, map[randomRoomIndex]);

            if (LocationRepository.SpecialRoomsCarcases[i].Name == "Home village")
            {
                StartRoomIndex = randomRoomIndex;
                Debug.Log("StartRoomIndex: " + StartRoomIndex);
            }
        }

        saver.SaveWorldToTXT("firstWorld", new WorldStorage(LocationsMap, map, startRoomIndex));

        IsWorldMapFull = true;
        Debug.Log("IsWorldMapFull=true");
    }

    public void CreateWorld()
    {
        saver = GetComponent<Saver>();
        LocationRepository = GetComponent<LocationRepository>();

        IsWorldMapFull = false;

        locationsStr = new LocationRepository().Locations;

        for (int i = 0; i < locationsStr.Count; i++)
        {
            locationsInt.Add(i);
        }
        
        LocationsMap = new int[WorldSide * WorldSide];

        for (int i = 0; i < WorldSide * WorldSide; i++)
        {
            LocationsMap[i] = locationsInt[locationsInt.Count - 1];
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

                StrFilling(new Vector2(centerX, centerY), points[i][e], points[i][e + 1], rand);
            }

            StrFilling(new Vector2(centerX, centerY), points[i][points[i].Count - 1], points[i][0], locationsInt[locationsInt.Count - i - 2]);
        }
        points.Reverse();

        // Перенос точек на карту
        for (int location = 0; location < points.Count; location++)
        {
            for (int i = 0; i < points[location].Count; i++)
            {
                int _X = (int) points[location][i].x;
                int _Y = (int) points[location][i].y;
                LocationsMap[100 * (_Y - 1) + _X] = location;
            }
        }
        
        for (int i = 0; i < LocationsMap.Length; i++) // qq типа пофиксил "err01"
        {
            if (LocationsMap[i] == 9)
            {
                LocationsMap[i] = LocationsMap[i - 1];
            }
        }
        
        StartCoroutine(RoomCreationBGProc(points));
    }


    public List<Vector2> GetNewApexes(List<Vector2> p, int centerX, int centerY)
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


    public void StrFilling(Vector2 center, Vector2 p0, Vector2 p1, int fill)
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
                int _ind = WorldSide * ((int)Mathf.Round(ny) - 1) + (int)Mathf.Round(nx);

                LocationsMap[_ind] = extraFill;

                nx += Mathf.Abs(w) > Mathf.Abs(h) ? d : z;
                ny += Mathf.Abs(w) > Mathf.Abs(h) ? z : d;
            }
        }

        for (int y = 1; y < WorldSide + 1; y++) // Заполнение
        {
            List<int> w = new List<int>();

            for (int i = WorldSide * (y - 1); i < WorldSide * y; y++)
            {
                w.Add(LocationsMap[i]);
            }

            if (w.Contains(extraFill))
            {
                List<int> foundPoints = new List<int>();

                for (int x = 0; x < w.Count; x++)
                {
                    if (w[x] == extraFill)
                    {
                        foundPoints.Add(WorldSide * (y - 1) + x);
                    }
                }

                for (int x = foundPoints[0]; x < foundPoints[foundPoints.Count - 1] + 1; x++)
                {
                    LocationsMap[x] = fill;
                }
            }
        }
    }


    public List<Tile> CreateRoomData(int locationIndex)
    {
        List<Tile> tiles = new List<Tile>();
        //Debug.Log(locationIndex); //qq
        LocationPattern lr = LocationRepository.LocationPatterns[locationIndex];
        List<Sprite> tilesRep = lr.Tiles;
        List<Sprite> bushesRep = lr.Bushes;
        List<Sprite> bigBushesRep = lr.BigBushes;
        List<Sprite> wallsRep = lr.Walls;

        int wallsCount = UnityEngine.Random.Range(0, 5);

        List<int> stonesArea = LocationRepository.StonesArea;
        List<int> stonesIndexes = new List<int>();

        for (int i = 0; i < wallsCount; i++)
        {
            int rand = UnityEngine.Random.Range(0, stonesArea.Count);
            stonesIndexes.Add(stonesArea[rand]);
            stonesArea.Remove(rand);
        }

        for (int i = 0; i < WorldSide; i++)
        {
            int tileIndex;
            bool isWall;
            List<int> bushesIndexes = new List<int>();
            List<Vector2Serializable> bushesPositions = new List<Vector2Serializable>();
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
                bushesPositions.Add(new Vector2Serializable(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f)));
            }

            tiles.Add(new Tile(tileIndex, isWall, bushesIndexes, bushesPositions, isBushBig));
        }

        return tiles;
    }


    public List<Tile> ChangeRoomToSpecial(int locationIndex, List<Tile> oldRoom)
    {
        List<Tile> newRoom = oldRoom;
        /*
        for (int i = 0; i < )
        {

        }
        */
        return newRoom;
    }
}

[Serializable] public class WorldStorage
{
    public int[] LocationMap;
    public List<List<Tile>> Map;
    public int StartRoomIndex;

    public WorldStorage() { }
    //public WorldStorage(List<List<Tile>> map) { }
    public WorldStorage(int[] locationMap, List<List<Tile>> map, int startRoomIndex)
    {
        LocationMap = locationMap;
        Map = map;
        StartRoomIndex = startRoomIndex;
    }
}