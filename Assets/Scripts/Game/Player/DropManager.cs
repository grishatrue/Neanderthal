using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropManager : MonoBehaviour
{
    public List<Droplist> droplist;
    public List<Dropperlist> dropperlist;

    public GameObject dropItem;

    // список предметов с ай ди как у существ
    void Start()
    {
        InventoryItems data = GetComponent<InventoryItems>();

        int dropCount = data.items.Count;

        for (int i = 0; i < dropCount; i++) // заполнение массива droplist
        {
            droplist[i].id = data.items[i].id;
            droplist[i].name = data.items[i].name;
            droplist[i].img = data.items[i].img;
        }
    }

    void Update()
    {

    }

    public void Die(string dropperName, Vector2 parantPos)
    {
        List<int> currentDropList = new List<int>();

        for (int i = 0; i < droplist.Count; i++) // отбор нужных предетов по имени врага (id этого предмета совпадает с id предмета из даты)
        {
            if (dropperName == droplist[i].dropper)
            {
                currentDropList.Add(droplist[i].id);
            }
        }

        int currentDropper = 0;

        for (int i = 0; i < dropperlist.Count; i++) // поиск нужного дроппера
        {
            if (dropperName == dropperlist[i].dropperitem.name)
            {
                currentDropper = i;
                break;
            }
        }

        int rand = Random.Range(1, dropperlist[currentDropper].maxDropCount); // рандомное число выпадающих предметов 

        for (int i = 0; i < rand; i++)
        {
            int id = currentDropList[Random.Range(0, currentDropList.Count)]; // рандомное id предмета (совпадает с id в дате)

            if (id != 0)
            {
                GameObject item = Instantiate(dropItem, parantPos, Quaternion.identity);
                item.GetComponent<SpriteRenderer>().sprite = droplist[id].img;
                item.name = id.ToString();
            }
        }
    }
}


[System.Serializable]
public class Droplist
{
    public int id;
    public string name;
    public Sprite img;
    public string dropper;
}

[System.Serializable]
public class Dropperlist
{
    public GameObject dropperitem;
    public int maxDropCount;
}

