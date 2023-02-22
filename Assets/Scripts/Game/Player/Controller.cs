using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

public class Controller : MonoBehaviour

{
    public float speed;
    private Rigidbody2D rb;
    private Animator anim;
    //public Inventory inv;
    private readonly string afk = "none";
    //public RoomManager rm;
    private float velocityLim = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        MovementSystem();
        Animator();
    }
    private void MovementSystem()
    {
        Vector2 sp = new Vector2();
        float moveHorisontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorisontal, moveVertical);

        if (Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.A) & Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.D) & Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.A) & Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.D) & Input.GetKey(KeyCode.S))
            {
                sp = (movement * speed * 0.3f);
            }
            else
            {
                sp = (movement * speed * 0.5f);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A) & Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.D) & Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.A) & Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.D) & Input.GetKey(KeyCode.S))
            {
                sp = (movement * speed * 0.7f);
            }
            else
            {
                sp = (movement * speed);
            }
        }
        rb.velocity = sp * velocityLim;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "bush")
        {
            velocityLim = 0.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "bush")
        {
            velocityLim = 1f;
        }
    }

    private void Animator()
    {
        //afk = inv.afk;
        if (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Go", true);
        }
        else
        {
            anim.SetBool("Go", false);
        }
        if (!Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.A))
            {
                anim.SetBool("Left", true);
                anim.SetBool("Right", false);
            }
            else
            {
                anim.SetBool("Left", false);
            }
        }
        if (!Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.D))
            {
                anim.SetBool("Right", true);
                anim.SetBool("Left", false);
            }
            else
            {
                anim.SetBool("Right", false);
            }
        }
        if (!Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.W))
            {
                anim.SetBool("Up", true);
                anim.SetBool("Down", false);
            }
            else
            {
                anim.SetBool("Up", false);
            }
        }
        if (!Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.S))
            {
                anim.SetBool("Down", true);
                anim.SetBool("Up", false);
            }
            else
            {
                anim.SetBool("Down", false);
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (afk == "none")
            {
                anim.SetBool("AttackArm", true);
            }
            else if (afk == "baton")
            {
                anim.SetBool("AttackBaton", true);
            }
        }
        else
        {
            anim.SetBool("AttackArm", false);
            anim.SetBool("AttackBaton", false);
        }
    }
}