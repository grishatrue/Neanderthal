using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameSceneLauncher : MonoBehaviour
{
    public void LaunchGameScene()
    {
        FileManager.SaveJSON(new WorldDataGenerator().GetNewWorldData());

        // ������� �� ������� �����

        // (��� �� ������� �����) �������� ��������� �������

    }
}
