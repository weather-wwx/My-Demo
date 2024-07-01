using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    private Boss boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //对玩家造成伤害
            boss.nowAttack += 10;
            collision.GetComponent<Character>().TakeDamage(boss);
        }
    }

    //得知发射对象
    public void GetLauncher(Boss enemy)
    {
        boss = enemy;
    }

    public void DestroyAnimation()
    {
        Destroy(this.gameObject);
    }
}
