using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMap : MonoBehaviour
{
    private int worldSide;

    public void ppp(int[] locMap)
    {
        worldSide = new WorldCreator().WorldSide;

        string outText = "";

        for (int y = 1; y < worldSide; y++)
        {
            for (int x = 1; x < worldSide; x++)
            {
                outText += locMap[(y - 1) * worldSide + x];
            }
        }

        GetComponent<Text>().text = outText;

    }
}
