using UnityEngine;

public class FileManager
{
    private static string savePath = Application.persistentDataPath + "/Saves/World Saves";

    public string SavePath { get => savePath; private set => savePath = value; }
}
