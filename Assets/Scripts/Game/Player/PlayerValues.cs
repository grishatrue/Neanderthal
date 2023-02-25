using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    public int health = 100;
    public int armor = 100;
    public int mana = 100;

/*
    public GameObject Cam;

    [HideInInspector]
    public Inventory inv;
    [HideInInspector]
    public RoomManager rm;
    [HideInInspector]
    public Vector2 activeRoom;
    [HideInInspector]
    public Transform plPos;

    void Start()
    {
        plPos = GetComponent<Transform>();

        inv = Cam.GetComponent<Inventory>();
        rm = Cam.GetComponent<RoomManager>();

        activeRoom = rm.activeRoom;
    }

    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision2D)
    {
        if (collision2D.gameObject.tag == "ItemRB")
        {
            inv.OnCollisionPlayer(collision2D);
        }

        if (collision2D.gameObject.tag == "Door")
        {
            RoomManager c = Cam.GetComponent<RoomManager>();
            string nm = c.rmObj.name;
            Destroy(c.rmObj); // Объект не удаляется, а очищается
            c.rmObj = new GameObject();
            c.rmObj.name = nm;

            int x = 5;

            if (collision2D.name == "Right")
            {
                activeRoom.x += 1;
                plPos.position = new Vector2(-x, 0);
            }
            else if (collision2D.name == "Left")
            {
                activeRoom.x -= 1;
                plPos.position = new Vector2(x, 0);
            }
            else if (collision2D.name == "Down")
            {
                activeRoom.y += 1;
                plPos.position = new Vector2(0, x);
            }
            else if (collision2D.name == "Up")
            {
                activeRoom.y -= 1;
                plPos.position = new Vector2(0, -x);
            }

            print("rm num: " + (activeRoom.y * c.worldSize - c.worldSize + activeRoom.x) + " Room pos:" +(activeRoom.x + ":" + activeRoom.y));
            c.LoadRoom("rm" + (activeRoom.y * c.worldSize - c.worldSize + activeRoom.x));
        }
    }*/
}
