using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private DoorSideEnum side;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}

public enum DoorSideEnum
{
    LEFT, UP, RIGHT, DOWN
}