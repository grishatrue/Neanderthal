using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataBase : MonoBehaviour
{
    public List<InventoryItem> InventoryItems = new List<InventoryItem>();
}

[System.Serializable]

public class InventoryItem
{
    public int Id;
    public string Name;
    public Sprite Img;
    public int Stack;
    public string Group;
    public int Level;
    public string Description;
}