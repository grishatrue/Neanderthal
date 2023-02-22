using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManagerPlayer : MonoBehaviour
{
    public SpriteRenderer player;
    public Transform pos;

    void Start()
    {
        
    }

    void Update()
    {
        player.sortingOrder = -(int)(Mathf.Round(pos.position.y * 10));
    }
}
