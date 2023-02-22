using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class Saver : MonoBehaviour
{
    private string savePath = "/Saves/World Saves";
    public string SavePath { get => savePath; private set => savePath = value; }

    private BinaryFormatter formatter;

    private void Awake()
    {
        SavePath = Application.persistentDataPath + SavePath;
        formatter = new BinaryFormatter();
    }

    public void SerialiseWorld(string fileName, object contents)
    {
        if (contents != null)
        {
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            if (!File.Exists(SavePath))
            {
                var file = File.Create(SavePath + "/" + fileName);
                formatter.Serialize(file, contents);
                file.Close();
                Debug.Log(SavePath + "/" + fileName + " - cохранён.");
            }
            else
            {
                Debug.Log("Ошибка сохранения: Файл с таким именем (" + fileName + ") уже существует и будет загружен!\n Путь: " + SavePath + "//" + fileName);
            }
        }
        else
            Debug.Log("Ошибка сохранения: ожидается имя файла.");
    }

    public void SaveWorldToTXT(string fileName, WorldStorage storage)
    {
        string data = "LocationMap{";
        for (int i = 0; i < storage.LocationMap.Length; i++)
        {
            data += storage.LocationMap[i] + ",";
        }
        data += "}\nMap{";
        for (int i = 0; i < storage.Map.Count; i++)
        {
            data += "tile(";


            for (int e = 0; e < storage.Map[i].Count; e++)
            {
                data += "TileIndex:" + storage.Map[i][e].TileIndex + "IsWall:" + storage.Map[i][e].IsWall.ToString() + "BushesIndexes{";
                for (int g = 0; g < storage.Map[i][e].BushesIndexes.Count; g++)
                {
                    data += storage.Map[i][e].BushesIndexes[g] + ",";
                }
                data += "}BushesPositions{";
                for (int j = 0; j < storage.Map[i][e].BushesPositions.Count; j++)
                {
                    data += "(" + storage.Map[i][e].BushesPositions[j].x + "," + storage.Map[i][e].BushesPositions[j].y + ")";
                }
                data += "}IsBushBig{";
                for (int h = 0; h < storage.Map[i][e].IsBushBig.Count; h++)
                {
                    data += storage.Map[i][e].IsBushBig[h].ToString() + ",";
                }
                data += "}";
            }


            data += "),";
        }
        data += "}\nStartRoomIndex:" + storage.StartRoomIndex;

        File.WriteAllText(SavePath, data);
    }
}
