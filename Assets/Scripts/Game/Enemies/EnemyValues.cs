using UnityEngine;
using UnityEngine.UI;

public class EnemyValues : MonoBehaviour
{
    public GameObject enemy;
    public DropManager drop;

    public int HP = 100;
    public Text thp;

    void Start()
    {

    }

    void Update()
    {
        thp.text = HP.ToString() + "/100";

        if (HP <= 0)
        {
            drop.Die(enemy.name, enemy.transform.position);
            Destroy(enemy);
        }
    }
}

