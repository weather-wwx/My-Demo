using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : BaseState
{
    private float targetX;
    private Vector3 moveDir;

    private float attackRateCounter;
    //private bool isAttack;

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.anim.SetBool("isWalk", true);
    }

    public override void LogicUptate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NpcState.Patrol);
            currentEnemy.attacker = null;
            return;
        }

        targetX = currentEnemy.attacker.position.x;

        //¼ÆÊ±Æ÷
        attackRateCounter -= Time.deltaTime;

        //ÅÐ¶Ï¹¥»÷¾àÀë
        if (Mathf.Abs(targetX - currentEnemy.transform.position.x) <= currentEnemy.attackRange)
        {
            currentEnemy.anim.SetBool("isWalk", false);
            //¹¥»÷
            currentEnemy.isAttack = true;
            if (!currentEnemy.isHurt)
                currentEnemy.rb.velocity = Vector2.zero;

            if (attackRateCounter <= 0)
            {
                currentEnemy.anim.SetTrigger("attack");
                attackRateCounter = currentEnemy.attackRate;
            }
        }
        else    //³¬³ö¹¥»÷·¶Î§
        {
            currentEnemy.anim.SetBool("isWalk", true);
            currentEnemy.isAttack = false;
        }

        //·¨Êõ¹¥»÷
        if (Mathf.Abs(targetX - currentEnemy.transform.position.x) > 6 && attackRateCounter <= 0)
        {
            currentEnemy.rb.velocity = Vector2.zero;
            currentEnemy.anim.SetTrigger("cast");
            attackRateCounter = currentEnemy.attackRate + 1;
        }

        moveDir.x = targetX - currentEnemy.transform.position.x;

        if (moveDir.x > 0 && !currentEnemy.isAttack)
        {
            moveDir.x = 1;
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (moveDir.x < 0 && !currentEnemy.isAttack)
        {
            moveDir.x = -1;
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isHurt && !currentEnemy.isDead && !currentEnemy.isAttack)
        {
            currentEnemy.rb.velocity = new Vector2(currentEnemy.currentSpeed * Time.deltaTime * moveDir.x, 0);
        }
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("isWalk", false);
    }
}
