using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    public SpriteRenderer enemy;
    public Transform pos;
    public Canvas enemyvalue;

    void Start()
    {

    }

    void Update()
    {
        //enemy.sortingOrder = -(int)(Mathf.Round(pos.position.y * 10));
        //enemyvalue.sortingOrder = enemy.sortingOrder;
    }
}
