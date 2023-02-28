using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameSceneLauncher : MonoBehaviour
{
    public void LaunchGameScene()
    {
        FileManager.SaveJSON(new WorldDataGenerator().GetNewWorldData());

        // Переход на игровую сцену

        // (уже на игровой сцене) Загрузка стартовой комнаты

    }
}
