using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 pointPos;
    private Vector2 currentPos;

    public float amplitude;
    public float speed;

    private bool direction = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pointPos = GetComponent<Transform>().position;

        rb.AddForce(new Vector2(Random.Range(-3, 3), Random.Range(-3, 3)));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (direction)
        {
            rb.velocity = new Vector3(0, speed);

            if (amplitude <= (currentPos.y - pointPos.y))
            {
                direction = !direction;
            }
        }
        else
        {
            rb.velocity = new Vector3(0, -speed);

            if (-amplitude >= (currentPos.y - pointPos.y))
            {
                direction = !direction;
            }
        }

        currentPos = GetComponent<Transform>().position;

    }
}
