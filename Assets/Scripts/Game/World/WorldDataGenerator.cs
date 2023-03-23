using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class WorldDataGenerator
{
    private int extraFill = 9;

    public WorldData GetNewWorldData()
    {
        int startRoomIndex = 3400;
        int worldSide = WorldAncillaryData.WorldSide;
        int[] locationMap = new int[] { };
        List<int> locationsInt = new List<int>();
        Dictionary<int, List<Vector2>> sortedLocMap = new Dictionary<int, List<Vector2>>();

        // Заполнение мира водой
        for (int i = 0; i <= (int)LocationEnum.SEA; i++) { locationsInt.Add(i); }
        locationMap = new int[worldSide * worldSide];
        for (int i = 0; i < worldSide * worldSide; i++) { locationMap[i] = locationsInt[locationsInt.Count - 1]; }
        List<Vector2> points3 = new List<Vector2>();
        List<Vector2> points6 = new List<Vector2>();
        List<Vector2> points12 = new List<Vector2>();
        List<Vector2> points24 = new List<Vector2>();
        List<List<Vector2>> points = new List<List<Vector2>>() { points3, points6, points12, points24 };
        List<Vector2> possiblePoints = new List<Vector2>();
        int spaceBetweenPoints = 5;

        // Выбор облости для первой локации
        while (possiblePoints.Count == 0)
        {
            possiblePoints = new List<Vector2>();

            for (int y = 45; y <= 55; y++)
                for (int x = 45; x <= 55; x++)
                    if (x < 45 + 2 || x > 55 - 2 || y < 45 + 2 || y > 55 - 2)
                        possiblePoints.Add(new Vector2(x, y));
        }

        // Генерация первых 3 точкек
        for (int i = 0; i < 3; i++)
        {
            Vector2 rp = possiblePoints[UnityEngine.Random.Range(0, possiblePoints.Count - 1)]; // первая random point
            points[0].Add(rp);

            for (int y = (int)rp[1] - spaceBetweenPoints; y < rp[1] + spaceBetweenPoints + 1; y++)
                for (int x = (int)rp[0] - spaceBetweenPoints; x < rp[0] + spaceBetweenPoints + 1; x++)
                    if (possiblePoints.Contains(new Vector2(x, y)))
                        possiblePoints.Remove(new Vector2(x, y));
        }

        // Получение вершин секторов
        int centerX = (int)((points3[0].x + points3[1].x + points3[2].x) / 3);
        int centerY = (int)((points3[0].y + points3[1].y + points3[2].y) / 3);
        Vector2 center = new Vector2(centerX, centerY); // Для вулкана
        for (int i = 0; i < 3; i++) { points[i + 1] = GetNewApexes(points[i], centerX, centerY); }

        // Заполнение секторов
        points.Reverse();

        for (int locInd = 0; locInd < points.Count; locInd++)
        {
            for (int e = 0; e < points[locInd].Count - 1; e++)
            {
                int randLoc;
                if (locInd == 1) { randLoc = locationsInt[locationsInt.Count - UnityEngine.Random.Range(1, 2) - 2]; }
                else { randLoc = locationsInt[locationsInt.Count - locInd - 2]; }
                FillInLines(new Vector2(centerX, centerY), points[locInd][e], points[locInd][e + 1], randLoc, ref locationMap);
            }

            FillInLines(new Vector2(centerX, centerY), points[locInd][points[locInd].Count - 1], points[locInd][0],
                locationsInt[locationsInt.Count - locInd - 2], ref locationMap); // Заполнение последнего сектора
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

        // Генерация комнат
        var tileIndexMap = GenerateRooms(points, locationMap);

        // Добавление спецлокаций
        var tileMap = SpecialRoomsPlacing(ref locationMap, ref tileIndexMap, ref startRoomIndex, center);

        return new WorldData(startRoomIndex, locationMap, tileIndexMap);
    }

    private List<List<Tile>> GenerateRooms(List<List<Vector2>> pointMap, int[] locationMap)
    {
        var rooms = new List<List<Tile>>();

        for (int i = 0; i < locationMap.Length; i++)
            rooms.Add(GetNewRoomData(locationMap[i]));

        return rooms;
    }

    private List<List<Tile>> SpecialRoomsPlacing(ref int[] locationMap, ref List<List<Tile>> tileMap, ref int startRoomIndex, Vector2 center)
    {
        var specialLocRes = WorldAncillaryData.GetSpecialLocationResources();
        var locRes = WorldAncillaryData.GetLocationResources();
        startRoomIndex = 0;
        var possiblePoints = new Dictionary<int, List<int>>();

        // разделить карты на карты локаций
        for (int i = 0; i < locationMap.Length; i++)
        {
            if (locationMap[i] != (int)LocationEnum.SEA)
            {
                if (!possiblePoints.ContainsKey(locationMap[i])) { possiblePoints[locationMap[i]] = new List<int>(); }
                possiblePoints[locationMap[i]].Add(i);
            }
        }

        for (int i = 0; i < WorldAncillaryData.SpecialRoomCarcases.Count; i++) // special rooms carcases
        {
            var currCarcase = WorldAncillaryData.SpecialRoomCarcases[i];

            for (int j = 0; j < currCarcase.roomsCount; j++) // rooms
            {
                int currentLocInd = (int)currCarcase.sourceLocation;
                int randRInd = possiblePoints[currentLocInd][UnityEngine.Random.Range(0, possiblePoints[currentLocInd].Count)];

                if (currCarcase.locName == SpecialLocationEnum.HOME_VILLAGE)
                    startRoomIndex = randRInd;
                
                if (currCarcase.locName == SpecialLocationEnum.VOLCANO)
                    randRInd = Mathf.RoundToInt(center.y * 100 - 1 + center.x);

                locationMap[randRInd] = (int)currCarcase.locName + 5; // test // qqq // починить размещение спецлокаций

                for (int ti = 0; ti < tileMap[randRInd].Count; ti++) // tile index
                {
                    int currTileInd = currCarcase.tilesIndexes[ti];

                    if (currTileInd != -1)
                    {
                        var newTName = specialLocRes[i].TilesSprites[currTileInd].name;
                        bool isWall = newTName.Split('-')[1].ToLower() == "wall" ? true : false;
                        Tile newTile = new Tile(currTileInd, isWall);
                        tileMap[randRInd][ti] = newTile;
                    }
                    else if (tileMap[randRInd][ti].isWall) // меняем стену на обычный тайл
                    {
                        int randBCount = UnityEngine.Random.Range(0, 4);
                        List<Bush> newBushes = new List<Bush>();

                        for (int bi = 0; bi < randBCount; bi++) // Bushes
                        {
                            bool isBBig = (new bool[] { false, true })[UnityEngine.Random.Range(0, 2)];
                            Vector2 bPos = new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
                            int bInd = isBBig ? 
                                UnityEngine.Random.Range(0, locRes[currentLocInd].BigBushes.Count) :
                                UnityEngine.Random.Range(0, locRes[currentLocInd].Bushes.Count);

                            newBushes.Add(new Bush(bInd, bPos, isBBig));
                        }

                        tileMap[randRInd][ti] = new Tile(currTileInd, false, newBushes);
                    }
                }

                // clear possibles
                for (int y = randRInd % 100 - 4; y < randRInd % 100 + 5; y++)
                    for (int x = randRInd / 100 - 4; x < randRInd / 100 + 5; x++)
                        if (possiblePoints[currentLocInd].Contains(y * 100 + x)) { possiblePoints[currentLocInd].Remove(y * 100 + x); }
            }
        }

        return tileMap;
    }

    private List<Vector2> GetNewApexes(List<Vector2> p, int centerX, int centerY)
    {
        int n = p.Count;

        // np - New points
        List<Vector2> np = new List<Vector2>(); // Массив точек контура новой локации
        List<Vector2> np1 = new List<Vector2>(); // Массив координат отрезков, из центра к вершине
        List<Vector2> np2 = new List<Vector2>(); // ... к стороне

        // k - коэффициенты увеличения удаленности вершин новых локаций от центра острова.
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

    private void FillInLines(Vector2 center, Vector2 p0, Vector2 p1, int fill, ref int[] locationMap)
    {
        int worldSide = WorldAncillaryData.WorldSide;
        int _extraFill = extraFill; // Символ предзаполнения
        List<Vector2> secPre = new List<Vector2> { center, p0, p1 }; // Сектор предзаполнения

        for (int i = 0; i < secPre.Count; i++) // Расчет данных о грани сектора заполнения и её заполнение
        {
            int n = 0; // var that is longer (h or w)
            int _ind = i <= 0 ? i + secPre.Count - 1 : i - 1;
            float h = secPre[i].y - secPre[_ind].y; // height
            float w = secPre[i].x - secPre[_ind].x; // width
            float lv = 0; // longer vector
            float sv = 0; // shorter vector
            float nx = secPre[_ind].x;
            float ny = secPre[_ind].y;

            if (Mathf.Abs(w) > Mathf.Abs(h))
            {
                n = (int)w;
                lv = w / Mathf.Abs(w);
                if (h == 0) { sv = 0; }
                else { sv = Mathf.Abs(h / w) * (h / Mathf.Abs(h)); }
            }
            else
            {
                n = (int)h;
                lv = h / Mathf.Abs(h);
                if (w == 0) { sv = 0; }
                else { sv = Mathf.Abs(w / h) * (w / Mathf.Abs(w)); }
            }

            for (int j = 1; j < Mathf.Abs(n) + 1; j++) // Предзаполнение грани сектора заполнения
            {
                int exInd = worldSide * ((int)Mathf.Round(ny) - 1) + (int)Mathf.Round(nx);
                locationMap[exInd] = _extraFill;
                nx += Mathf.Abs(w) > Mathf.Abs(h) ? lv : sv;
                ny += Mathf.Abs(w) > Mathf.Abs(h) ? sv : lv;
            }
        }

        // Выбор области сектора
        List<int> xp = new List<int>(); // width sector
        List<int> yp = new List<int>(); // height sector

        foreach (Vector2 i in secPre)
        {
            xp.Add(Mathf.RoundToInt(i.x));
            yp.Add(Mathf.RoundToInt(i.y));
        }

        xp.Sort();
        yp.Sort();

        for (int y = yp[0]; y < yp[yp.Count - 1] + 1; y++)
        {
            List<int> strInd = new List<int>();
            List<int> strVal = new List<int>();

            for (int x = xp[0]; x < xp[xp.Count - 1] + 1; x++)
            {
                strInd.Add(100*(y-1)+x);
                strVal.Add(locationMap[100*(y-1)+x]);
            }

            int i1 = strVal.FindIndex(_ => _ == _extraFill);
            int i2 = strVal.FindLastIndex(_ => _ == _extraFill);
            for (int x = i1; x < i2 + 1; x++) { locationMap[strInd[x]] = fill; }
        }
    }

    private List<Tile> GetNewRoomData(int locationIndex)
    {
        List<Tile> newRoomData = new List<Tile>();
        var resourcesList = WorldAncillaryData.GetLocationResources();
        LocationResources locationRess = resourcesList[locationIndex];
        int stonesCount = UnityEngine.Random.Range(0, 5);
        List<int> stonesIndexes = new List<int>();
        List<int> possibleInds = new List<int>();

        for (int i = 0; i < WorldAncillaryData.StonesArea.Count; i++) // possible area assignment
            if (WorldAncillaryData.StonesArea[i] == 1) { possibleInds.Add(i); }

        for (int i = 0; i < stonesCount; i++) // stones additing
        {
            int randInd = UnityEngine.Random.Range(0, possibleInds.Count);
            possibleInds.Remove(randInd);
            stonesIndexes.Add(randInd);
        }

        for (int i = 0; i < WorldAncillaryData.WorldSide; i++) // room // 10x10 tiles = 100 = world side
        {
            Tile newTile = new Tile();
            newTile.bushes = new List<Bush>();

            if (stonesIndexes.Contains(i)) // is curr tile is Stone?
            {
                newTile.tileIndex = UnityEngine.Random.Range(0, locationRess.Walls.Count);
                newTile.isWall = true;
            }
            else
            {
                newTile.tileIndex = UnityEngine.Random.Range(0, locationRess.TilesSprites.Count);
                newTile.isWall = false;
            }

            int randBushesCount = UnityEngine.Random.Range(0, 5);

            for (int e = 0; e < randBushesCount; e++) // Bushes
            {
                int newInd;
                bool isBushBig = new bool[] { false, true }[UnityEngine.Random.Range(0, 2)];
                if (isBushBig) { newInd = UnityEngine.Random.Range(0, locationRess.BigBushes.Count); }
                else { newInd = UnityEngine.Random.Range(0, locationRess.Bushes.Count); }
                Vector2 newBushPosition = new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
                newTile.bushes.Add(new Bush(newInd, newBushPosition, isBushBig));
            }

            newRoomData.Add(newTile);
        }

        return newRoomData;
    }
}
