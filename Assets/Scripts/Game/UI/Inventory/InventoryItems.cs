using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItems : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
}

[System.Serializable]
public class InventoryItem
{
    public int id;
    public string name;
    public Sprite img;
    public int stack;
    public string group;
    public int level;
    public string description;
}