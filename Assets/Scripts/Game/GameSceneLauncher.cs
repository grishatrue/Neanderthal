using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GameSceneLauncher : MonoBehaviour
{
    [SerializeField] private Text textMap;

    private void Start()
    {
        LaunchGameScene();
    }

    public void LaunchGameScene()
    {
        var newWD = new WorldDataGenerator().GetNewWorldData();
        textMap.text = GetTextMapAtWorldData(newWD.LocationIndexMap);
        // FileManager.SaveJSON(newWD);

        // Переход на игровую сцену
        // (уже на игровой сцене) Загрузка стартовой комнаты
    }

    private string GetTextMapAtWorldData(int[] locmap)
    {
        var strL = new List<string>();
        for (int i = 0; i < 10_000; i++)
        {
            strL.Add(locmap[i].ToString());
        }

        var newstr = "";
        for (int i = 0; i < 10_000; i++) 
        {
            if (strL[i] == "4") { strL[i] = " "; }
            if (i % 100 == 0 && i != 0) { newstr += "\n"; } 
            newstr += strL[i] + "  ";
        }
        return newstr;
    }
}
