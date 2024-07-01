using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    private Player player;
    public E_NowAttackSate attackSate;
    public UnityEvent OnShake;


    private void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            player.nowAttackSate = attackSate;
            switch (player.nowAttackSate)
            {
                case E_NowAttackSate.Attack2:
                    player.nowAttack = player.nowAttack + 5;
                    break;
                case E_NowAttackSate.Attack3:
                    player.nowAttack = player.baseAttack + 10;
                    break;
                case E_NowAttackSate.AirAttack2:
                    player.nowAttack = player.baseAttack + 5;
                    break;
                case E_NowAttackSate.AirAttack3:
                    player.nowAttack = player.baseAttack + 10;
                    player.attackResilience += 10;
                    break;
            }
            OnShake?.Invoke();
            collision.GetComponent<Character>()?.TakeDamage(player);
        }
    }
}
