using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    [Header("Глобальные объекты")]
    public InventoryItems data;
    public Camera cam;
    public EventSystem es;

    [Header("Массивы")]
    public List<ItemInventory> items = new List<ItemInventory>();
    public List<QuickItems> qitems = new List<QuickItems>();
    public List<Vector3> ItemsPositions;

    [Header("Структура меню")] // qqq Задать поля под панель карты и кнопки переключения.
    public GameObject backGround;
    public GameObject MainInventory;
    public GameObject InfoImg;
    public GameObject InfoText;
    public GameObject QuickInventory;
    public GameObject Map;

    [Header("Переменные")]
    public ItemInventory currentItem;
    public int currentID;
    public int maxCount; //of grafical elements
    public Vector3 offset;
    public string defaultText = "(Наведите мышь на предмет, чтобы увидть информацию о нем)";
    public int body;
    public int head;
    public int legs;
    public string afk;

    [Header("Другие объекты")]
    public GameObject gameObjShow;
    public RectTransform movingObject;
    public GameObject Player;
    public GameObject ItemRB;

    public void Start()
    {
        if (items.Count == 0)
        {
            AddGraphics();
        }
        for (int i = 0; i < 33; i++)
        {
            switch (i)
            {
                default:
                    AddItem(i, data.items[0], 0);
                    break;
                case 25:
                    AddItem(i, data.items[25], 0);
                    break;
                case 26:
                    AddItem(i, data.items[24], 0);
                    break;
                case 27:
                    AddItem(i, data.items[26], 0);
                    break;
                case 28:
                    AddItem(i, data.items[30], 0);
                    break;
                case 29:
                    AddItem(i, data.items[23], 0);
                    break;
                case 30:
                    AddItem(i, data.items[27], 0);
                    break;
                case 31:
                    AddItem(i, data.items[28], 0);
                    break;
                case 32:
                    AddItem(i, data.items[29], 0);
                    break;
            }
            
        }
        for (int i = 0; i < 25; i++) //тест, заполнить рандомные ячейки
        {
            int var1 = Random.Range(0, data.items.Count - 9);
            AddItem(i, data.items[var1], Random.Range(1, data.items[var1].stack));
        }
        UpdateInventory();
        qAddGraphics();

        for (int i = 0; i < 33; i++)
        {
            Vector3 itemPos = items[i].itemGameObj.GetComponent<RectTransform>().position;

            ItemsPositions.Add(itemPos);
        }
    }

    public void Update()
    {
        Vector3 pos = Input.mousePosition + offset;
        pos.z = MainInventory.GetComponent<RectTransform>().position.z;
        movingObject.position = cam.ScreenToWorldPoint(pos);

        Vector3 mousePos = movingObject.position; // позиция мыши must be с учетом сдвигов локальной системы координат (панели)
        float itemSize = 0.38f;

        for (int i = 0; i < items.Count; i++)
        {
            Vector3 itemPos = ItemsPositions[i];

            InventoryItem itemImg = data.items[items[i].id];

            if (mousePos.x >= itemPos.x - itemSize & mousePos.x <= itemPos.x + itemSize & mousePos.y >= itemPos.y - itemSize & mousePos.y <= itemPos.y + itemSize)
            {
                InfoImg.GetComponent<Image>().sprite = itemImg.img;
                InfoText.GetComponent<Text>().text = items[i].description;
                break;
            }
            else
            {
                InfoImg.GetComponent<Image>().sprite = data.items[0].img;
                InfoText.GetComponent<Text>().text = defaultText;
            }
           
        }

        if (currentID != -1)
        {
            MoveObject();
        }

        KeyboardCheck();

        AfkSwitch();
        qUpdateInventory();
        PlayerInventory();
    }

    public void KeyboardCheck()
    {
        // qqq Добавить логику переключения вкладок/окон (инвентарь, карта) ну и исправить то что тут написано, так как сам не понял что написал ваще... 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            backGround.SetActive(!backGround.activeSelf);
            QuickInventory.SetActive(!backGround.activeSelf);
            MainInventory.SetActive(!MainInventory.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!backGround.activeSelf)
            {
                backGround.SetActive(!backGround.activeSelf);
                QuickInventory.SetActive(!backGround.activeSelf);
            }
            
            MainInventory.SetActive(backGround.activeSelf);
            Map.SetActive(!MainInventory.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!backGround.activeSelf)
            {
                backGround.SetActive(!backGround.activeSelf);
                QuickInventory.SetActive(!backGround.activeSelf);
            }

            Map.SetActive(backGround.activeSelf);
            MainInventory.SetActive(!Map.activeSelf);
        }
    }
    public void OnCollisionPlayer(Collider2D collision2D)
    {
        GameObject item = collision2D.gameObject;
        int id = int.Parse(item.name);

        Destroy(item); //

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == int.Parse(item.name)) // стак
            {
                if (items[i].count < data.items[id].stack)
                {
                    items[i].count += 1;
                    break;
                }
                else // новая ячейка
                {
                    for (int e = 0; e < items.Count; e++)
                    {
                        if (items[e].id == 0)
                        {
                            AddItem(e, data.items[id], 1);
                            break;
                        }
                    }
                }
            }
            else if (items[i].id == 0) // новая ячейка
            {
                AddItem(i, data.items[id], 1);
                break;
            }
        }

        UpdateInventory();
    }

    public void SearchForSameItem(InventoryItem item, int count)
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (items[i].id == item.id)
            {
                if (items[0].count < item.stack)
                {
                    items[i].count += count;

                    if (items[i].count > item.stack)
                    {
                        count = items[i].count - item.stack;
                        items[i].count = item.stack;
                    }
                    else
                    {
                        count = 0;
                        i = maxCount;
                    }
                }
            }
        }
        if (count > 0)
        {
            for (int i = 0; i < maxCount; i++)
            {
                if (items[i].id == 0)
                {
                    AddItem(i, item, count);
                    i = maxCount;
                }
            }
        }
    }

    public void AddItem(int elementInInventory, InventoryItem itemFromData, int count)
    {
        int id = elementInInventory;
        InventoryItem item = itemFromData;

        items[id].id = item.id;
        items[id].count = count;
        items[id].itemGameObj.GetComponent<Image>().sprite = item.img;
        items[id].group = item.group;
        items[id].level = item.level;
        items[id].description = item.description;

        if (count > 1 & item.id != 0)
        {
            items[id].itemGameObj.GetComponentInChildren<Text>().text = count.ToString();
        }
        else
        {
            items[id].itemGameObj.GetComponentInChildren<Text>().text = "";
        }
    }


    public void AddInventoryItem(int id, ItemInventory invItem)
    {
        items[id].id = invItem.id;
        items[id].count = invItem.count;
        items[id].itemGameObj.GetComponent<Image>().sprite = data.items[invItem.id].img;
        items[id].group = invItem.group;
        items[id].level = invItem.level;
        items[id].description = invItem.description;

        if (invItem.count > 1 && invItem.id != 0)
        {
            items[id].itemGameObj.GetComponentInChildren<Text>().text = invItem.count.ToString();
        }
        else
        {
            items[id].itemGameObj.GetComponentInChildren<Text>().text = "";
        }
    }

    public void AddGraphics()
    {
        float xPos = 0;
        float yPos = 0;

        for (int i = 0; i < 25;)
        {
            yPos -= 40;
            xPos = 10;

            for (int e = 0; e < 5; e++)
            {
                xPos += 40;
                GameObject newItem = Instantiate(gameObjShow, MainInventory.transform) as GameObject;

                newItem.name = i.ToString();

                ItemInventory ii = new ItemInventory();
                ii.itemGameObj = newItem;

                RectTransform rt = newItem.GetComponent<RectTransform>();
                rt.localPosition = new Vector3(xPos, yPos, 0);
                rt.localScale = new Vector3(1, 1, 1);
                newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

                Button tempButton = newItem.GetComponent<Button>();

                tempButton.onClick.AddListener(delegate { SelectObject(); });

                items.Add(ii);

                i = i + 1;
            }
        }

        xPos = 500;
        yPos = 0;

        for (int i = 25; i < 30; i++)
        {
            yPos -= 40;
            GameObject newItem = Instantiate(gameObjShow, MainInventory.transform) as GameObject;

            newItem.name = i.ToString();

            ItemInventory ii = new ItemInventory();
            ii.itemGameObj = newItem;

            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(xPos, yPos, 0);
            rt.localScale = new Vector3(1, 1, 1);
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            Button tempButton = newItem.GetComponent<Button>();

            tempButton.onClick.AddListener(delegate { SelectObject(); });

            items.Add(ii);
        }

        xPos = 300;
        yPos = -270;

        for (int i = 30; i < 33; i++)
        {
            xPos += 40;
            GameObject newItem = Instantiate(gameObjShow, MainInventory.transform) as GameObject;

            newItem.name = i.ToString();

            ItemInventory ii = new ItemInventory();
            ii.itemGameObj = newItem;

            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(xPos, yPos, 0);
            rt.localScale = new Vector3(1, 1, 1);
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            Button tempButton = newItem.GetComponent<Button>();

            tempButton.onClick.AddListener(delegate { SelectObject(); });

            items.Add(ii);
        }
    }

    public void qAddGraphics()
    {
        float xPos = 10;
        for (int i = 0; i < 3; i++)
        {
            GameObject newItem = Instantiate(gameObjShow, QuickInventory.transform);

            newItem.name = i.ToString();

            QuickItems ii = new QuickItems();
            ii.qitemgo = newItem;

            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(xPos, 40, 0);
            rt.localScale = new Vector3(1, 1, 1);
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            qitems.Add(ii);

            xPos += 40;
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id != 0 && items[i].count > 1)
            {
                items[i].itemGameObj.GetComponentInChildren<Text>().text = items[i].count.ToString();
            }
            else
            {
                items[i].itemGameObj.GetComponentInChildren<Text>().text = "";
            }

            items[i].itemGameObj.GetComponent<Image>().sprite = data.items[items[i].id].img;
        }
    }

    /*public void SelectObject() // СЫСКАТЬ ЗДЕСЬ!!! (СЫС)
    {
        if (currentID == -1)
        {
            currentID = int.Parse(es.currentSelectedGameObject.name); // забираем предмет



            if (items[currentID].id != 0 & items[currentID].id < 23)
            {
                currentItem = CopyInventoryItem(items[currentID]);
                movingObject.gameObject.SetActive(true);
                movingObject.GetComponent<Image>().sprite = data.items[currentItem.id].img;

                RestrictionInventory();
                AddItem(currentID, data.items[0], 0); // клетка заменяется пустой клеткой
            }
            else
            {
                currentID = -1;
            }
        }
        else
        {
            ItemInventory II = items[int.Parse(es.currentSelectedGameObject.name)];

            for (int i = 25; i < 33; i++)
            {
                items[i].itemGameObj.SetActive(true);
            }

            if (currentItem.id != II.id)
            {
                //AddInventoryItem(currentID, II);
                AddInventoryItem(int.Parse(es.currentSelectedGameObject.name), currentItem);
                SelectObject();
            }
            else
            {
                if (II.count + currentItem.count <= data.items[currentItem.id].stack)
                {
                    II.count += currentItem.count;
                }
                else
                {
                    AddItem(currentID, data.items[II.id], II.count + currentItem.count - data.items[currentItem.id].stack);

                    II.count = data.items[currentItem.id].stack;

                }
                II.itemGameObj.GetComponentInChildren<Text>().text = II.count.ToString();
                UpdateInventory();
            }

            currentID = -1;

            movingObject.gameObject.SetActive(false);
        }
    }*/
    public void SelectObject()
    {
        int selectedId = int.Parse(es.currentSelectedGameObject.name);
        if (currentID == -1)
        {
            currentID = selectedId; // забираем предмет



            if (items[currentID].id != 0 & items[currentID].id < 23)
            {
                currentItem = CopyInventoryItem(items[selectedId]);
                movingObject.gameObject.SetActive(true);
                movingObject.GetComponent<Image>().sprite = data.items[currentItem.id].img;

                RestrictionInventory();
                switch (currentID)
                {
                    default:
                        AddItem(currentID, data.items[0], 0);
                        break;
                    case 25:
                        AddItem(currentID, data.items[25], 0);
                        break;
                    case 26:
                        AddItem(currentID, data.items[24], 0);
                        break;
                    case 27:
                        AddItem(currentID, data.items[26], 0);
                        break;
                    case 28:
                        AddItem(currentID, data.items[30], 0);
                        break;
                    case 29:
                        AddItem(currentID, data.items[23], 0);
                        break;
                    case 30:
                        AddItem(currentID, data.items[27], 0);
                        break;
                    case 31:
                        AddItem(currentID, data.items[28], 0);
                        break;
                    case 32:
                        AddItem(currentID, data.items[29], 0);
                        break;
                }
            }
            else
            {
                currentID = -1;
            }
        }
        else
        {
            for (int i = 25; i < 33; i++)
            {
                items[i].itemGameObj.SetActive(true);
            }

            if (items[selectedId].id == 0 | items[selectedId].id >= 23)
            {
                AddInventoryItem(selectedId, currentItem);
                movingObject.gameObject.SetActive(false);
                currentID = -1;
                UpdateInventory();
            }
            else
            {
                if (items[selectedId].id != currentItem.id)
                {
                    ItemInventory a = currentItem;
                    currentItem = CopyInventoryItem(items[selectedId]);
                    movingObject.gameObject.SetActive(true);
                    movingObject.GetComponent<Image>().sprite = data.items[items[selectedId].id].img;
                    AddInventoryItem(selectedId, a);
                    RestrictionInventory();
                    UpdateInventory();
                }
                else
                {
                    if (items[selectedId].count + currentItem.count <= data.items[currentItem.id].stack)
                    {
                        items[selectedId].count += currentItem.count;
                        movingObject.gameObject.SetActive(false);
                        currentID = -1;
                    }
                    else
                    {
                        currentItem.count = items[selectedId].count + currentItem.count - data.items[currentItem.id].stack;
                        items[selectedId].count = data.items[currentItem.id].stack;
                    }
                    UpdateInventory();
                }
            }
        }
    }


    public void MoveObject()
    {
        Vector3 pos = Input.mousePosition + offset;
        pos.z = MainInventory.GetComponent<RectTransform>().position.z;
        movingObject.position = cam.ScreenToWorldPoint(pos);

        if (!MainInventory.activeSelf) // qqq Тут можно изменить. Проверять не бекграунд, а активен ли сам инвентарь, так будет лучше наверное. 
        {
            movingObject.gameObject.SetActive(false);
            currentID = -1;
            for (int i = 0; i < 33; i++)
            {
                if (items[i].id == 0)
                {
                    AddItem(i, data.items[currentItem.id], currentItem.count);
                    UpdateInventory();
                    break;
                }
            }
        }
    }


    public ItemInventory CopyInventoryItem(ItemInventory old)
    {
        ItemInventory New = new ItemInventory();

        New.id = old.id;
        New.itemGameObj = old.itemGameObj;
        New.count = old.count;
        New.group = old.group;
        New.level = old.level;
        New.description = old.description;

        return New;
    }

    public void qUpdateInventory()
    {
        for (int i = 0; i < 3; i++)
        {
            qitems[i].id = items[30 + i].id;
            qitems[i].count = items[30 + i].count;
            qitems[i].qitemgo.GetComponent<Image>().sprite = items[30 + i].itemGameObj.GetComponent<Image>().sprite;
            qitems[i].group = items[30 + i].group;
            qitems[i].level = items[30 + i].level;
            qitems[i].description = items[30 + i].description;
            if (qitems[i].id != 0 && qitems[i].count > 1)
            {
                qitems[i].qitemgo.GetComponentInChildren<Text>().text = qitems[i].count.ToString();
            }
            else
            {
                qitems[i].qitemgo.GetComponentInChildren<Text>().text = "";
            }
        }
    }
    public void PlayerInventory()
    {
        switch (items[25].id)
        {
            default:
                head = 0;
                break;
            case 16:
                head = 1;
                break;
            case 17:
                head = 2;
                break;
        }
        switch (items[26].id)
        {
            default:
                body = 0;
                break;
            case 11:
                body = 1;
                break;
            case 12:
                body = 2;
                break;
            case 13:
                body = 3;
                break;

        }
        switch (items[27].id)
        {
            default:
                legs = 0;
                break;
            case 14:
                legs = 1;
                break;
            case 15:
                legs = 2;
                break;
        }
    }
    public void RestrictionInventory()
    {
        switch (currentItem.group)
        {
            default:
                for (int i = 25; i < 33; i++)
                {
                    items[i].itemGameObj.SetActive(false);
                }
                break;
            case "head":
                for (int i = 25; i < 33; i++)
                {
                    if (i != 25)
                    {
                        items[i].itemGameObj.SetActive(false);
                    }
                }
                break;
            case "body":
                for (int i = 25; i < 33; i++)
                {
                    if (i != 26)
                    {
                        items[i].itemGameObj.SetActive(false);
                    }
                }
                break;
            case "legs":
                for (int i = 25; i < 33; i++)
                {
                    if (i != 27)
                    {
                        items[i].itemGameObj.SetActive(false);
                    }
                }
                break;
            case "weapon":
                for (int i = 25; i < 33; i++)
                {
                    if (i != 28)
                    {
                        items[i].itemGameObj.SetActive(false);
                    }
                }
                break;
            case "artifacts":
                for (int i = 25; i < 33; i++)
                {
                    if (i != 29)
                    {
                        items[i].itemGameObj.SetActive(false);
                    }
                }
                break;
            case "hot keys":
                for (int i = 25; i < 30; i++)
                {
                    items[i].itemGameObj.SetActive(false);
                }
                break;
        }
    }
    public void AfkSwitch()
    {
        switch (data.items[items[28].id].name)
        {
            case "baton":
                afk = "baton";
                break;
            case "axe":
                afk = "axe";
                break;
            case "stone":
                afk = "stone";
                break;
            case "spear":
                afk = "spear";
                break;
            case "dart":
                afk = "dart";
                break;
            default:
                afk = "none";
                break;
        }
    }
}

[System.Serializable]

public class ItemInventory
{
    public int id;
    public GameObject itemGameObj;
    public string group;
    public int count;
    public int level;
    public string description;
}

[System.Serializable]

public class QuickItems
{
    public int id;
    public GameObject qitemgo; // quick item game object
    public string group;
    public int count;
    public int level;
    public string description;
}