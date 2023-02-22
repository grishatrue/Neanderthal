using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FightController : MonoBehaviour
{
    public EnemyValues e_hp; // hp of enemy
    public Transform radius_pos; // radius pos
    public Inventory inv;
    public float radius;
    int n;
    public int delay;
    public string afk;
    void Start()
    {
        n = delay;
    }

    void Update()
    {
        afk = inv.afk;
        if (n < delay)
        {
            n += 1;
        }
        switch (afk)
        {
            case "baton":
                aBaton();
                break;
            case "stone":
                aStone();
                break;
            case "axe":
                aAxe();
                break;
            case "spear":
                aSpear();
                break;
            case "dart":
                aDart();
                break;
            default:
                if (n == delay)
                {
                    if (Vector2.Distance(transform.position, radius_pos.position) <= radius & Input.GetKeyDown(KeyCode.Space))
                    {
                        e_hp.HP -= 5;
                        n = 0;
                    }
                }
                break;
        }
    }
    public void aBaton()
    {
        if (n == delay)
        {
            if (Vector2.Distance(transform.position, radius_pos.position) <= radius & Input.GetKeyDown(KeyCode.Space))
            {
                e_hp.HP -= 10;
                n = 0;
            }
        }
    }
    public void aStone()
    {
        if (n == delay)
        {
            if (Vector2.Distance(transform.position, radius_pos.position) <= radius & Input.GetKeyDown(KeyCode.Space)) // дальний
            {
                e_hp.HP -= 5;
                n = 0;
            }
        }
    }
    public void aAxe()
    {
        if (n == delay)
        {
            if (Vector2.Distance(transform.position, radius_pos.position) <= radius & Input.GetKeyDown(KeyCode.Space))
            {
                e_hp.HP -= 15;
                n = 0;
            }
        }
    }
    public void aSpear()
    {
        if (n == delay)
        {
            if (Vector2.Distance(transform.position, radius_pos.position) <= radius & Input.GetKeyDown(KeyCode.Space))
            {
                e_hp.HP -= 20;
                n = 0;
            }
        }
    }
    public void aDart()
    {
        if (n == delay)
        {
            if (Vector2.Distance(transform.position, radius_pos.position) <= radius & Input.GetKeyDown(KeyCode.Space)) // дальний
            {
                e_hp.HP -= 15;
                n = 0;
            }
        }
    }
}
