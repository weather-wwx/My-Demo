using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private float moveSpeed;
    private GameObject fire;
    private Boss boss;


    void Start()
    {
        moveSpeed = 10f;
        fire = Resources.Load<GameObject>("Prefabs/FireEffect");
    }

    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector2.down, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //对玩家造成伤害
            boss.nowAttack += 20;
            collision.GetComponent<Character>().TakeDamage(boss);

            //生成爆炸特效
            Instantiate(fire, new Vector3(transform.position.x, -2, 0), Quaternion.identity);
            Destroy(this.gameObject);
        }

        if(collision.gameObject.layer == 6)
        {
            Instantiate(fire, new Vector3(transform.position.x, -2, 0), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }


    //得知发射对象
    public void GetLauncher(Boss enemy)
    {
        boss = enemy;
    }
}
