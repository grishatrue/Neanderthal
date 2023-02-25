using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class HawkController : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public Transform player;

    public PlayerValues PlHpConnection;
    int n = 0;
    public int delay;
    public Animator anim;
    public Rigidbody2D rb;
    public Inventory inv;
    private int body;
    private int legs;
    private int head;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 movement = new Vector2((player.position.x / Mathf.Abs(player.position.x)) * speed, (player.position.y / Mathf.Abs(player.position.y)) * speed);
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            rb.velocity = (movement);
            anim.SetBool("HAttackLeft", false);
            anim.SetBool("HAttackRight", false);
            anim.SetBool("HAttackUp", false);
            anim.SetBool("HAttackDown", false);
        }
        else
        {
            rb.velocity = (movement * 0);
            anim.SetBool("HAttackLeft", true);
            anim.SetBool("HAttackRight", true);
            anim.SetBool("HAttackUp", true);
            anim.SetBool("HAttackDown", true);
            if (n < delay)
            {
                n += 1;
            }
            if (n == delay)
            {
                body = inv.body;
                legs = inv.legs;
                head = inv.head;
                PlHpConnection.health -= (10 - legs - body - head);
                n = 0;
            }

        }

        float X2 = player.position.x - transform.position.x;
        float Y2 = player.position.y - transform.position.y;
        if (X2 > 0)
        {
            if (Y2 > 0)
            {
                if (X2 > Y2) // right
                {
                    anim.SetBool("HGoLeft", false);
                    anim.SetBool("HGoUp", false);
                    anim.SetBool("HGoRight", true);
                    anim.SetBool("HGoDown", false);
                }
                else // up
                {
                    anim.SetBool("HGoUp", true);
                    anim.SetBool("HGoLeft", false);
                    anim.SetBool("HGoRight", false);
                    anim.SetBool("HGoDown", false);
                }
            }
            else
            {
                if (X2 > -Y2) // right
                {
                    anim.SetBool("HGoRight", true);
                    anim.SetBool("HGoLeft", false);
                    anim.SetBool("HGoUp", false);
                    anim.SetBool("HGoDown", false);
                }
                else // down
                {
                    anim.SetBool("HGoDown", true);
                    anim.SetBool("HGoLeft", false);
                    anim.SetBool("HGoUp", false);
                    anim.SetBool("HGoRight", false);
                }
            }
        }
        else // x -
        {
            if (Y2 > 0)
            {
                if (-X2 > Y2) // left
                {
                    anim.SetBool("HGoRight", false);
                    anim.SetBool("HGoLeft", true);
                    anim.SetBool("HGoUp", false);
                    anim.SetBool("HGoDown", false);
                }
                else // up
                {
                    anim.SetBool("HGoUp", true);
                    anim.SetBool("HGoLeft", false);
                    anim.SetBool("HGoRight", false);
                    anim.SetBool("HGoDown", false);
                }
            }
            else
            {
                if (-X2 > -Y2) // left
                {
                    anim.SetBool("HGoLeft", true);
                    anim.SetBool("HGoUp", false);
                    anim.SetBool("HGoRight", false);
                    anim.SetBool("HGoDown", false);
                }
                else // down
                {
                    anim.SetBool("HGoDown", true);
                    anim.SetBool("HGoLeft", false);
                    anim.SetBool("HGoUp", false);
                    anim.SetBool("HGoRight", false);
                }
            }
        }
    }
}
