using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GameSceneLauncher : MonoBehaviour
{
    [SerializeField] private Text textMap;
    [SerializeField] private SceneSwitcher sceneSwitcher;

    private void Start()
    {
        WorldData newWD = new WorldDataGenerator().GetNewWorldData();
        textMap.text = GetTextMapAtWorldData(newWD.LocationIndexMap); // Для теста // вывод карты символами
        FileManager.SaveJSON(newWD);
        WordDataHolder.world = newWD;
    }

    public void LaunchGameScene()
    {
        sceneSwitcher.GoToSceneAtName("GameScene");
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
            if (int.Parse(strL[i]) > 4) { strL[i] = "+"; }
            if (strL[i] == "4") { strL[i] = " "; }
            if (i % 100 == 0 && i != 0) { newstr += "\n"; } 
            newstr += strL[i] + "  ";
        }
        return newstr;
    }
}
