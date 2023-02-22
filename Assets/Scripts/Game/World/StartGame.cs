using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private string worldName = "DefaultName";
    public string WorldName { get => worldName; set => worldName = value; }


    public WorldCreator WorldCreator;
    public Saver Saver;
    public Loader Loader;
    public TextMap TextMap;
    public MiniMapSpawner MapSpawner;

    public void Start()
    {
        Saver = GetComponent<Saver>();
        Loader = GetComponent<Loader>();

        Invoke("StartWorldCreation", 0.5f);
        print("after Invoke()");
        //Loader.DeserialiseWorld(Saver.SavePath, WorldName);
        //Loader.LoadRoom(WorldCreator.StartRoomIndex);

        //MapSpawner.SpawnMap();
        //TextMap.ppp(WorldCreator.LocationsMap);

    }

    void StartWorldCreation()
    {
        WorldCreator.CreateWorld();
    }
}
