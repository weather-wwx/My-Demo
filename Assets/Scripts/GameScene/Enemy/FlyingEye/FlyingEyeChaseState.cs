using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeChaseState : BaseState
{
    private Vector3 target;
    private Vector3 moveDir;

    private float attackRateCounter;
    private bool isAttack;

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.anim.SetBool("chase", true);
    }

    public override void LogicUptate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.attacker = null;
            currentEnemy.SwitchState(NpcState.Patrol);
            return;
        }

        target = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 1.5f, 0);
        //¼ÆÊ±Æ÷
        attackRateCounter -= Time.deltaTime;

        //ÅÐ¶Ï¹¥»÷¾àÀë
        if (Mathf.Abs(target.x - currentEnemy.transform.position.x) <= currentEnemy.attackRange && Mathf.Abs(target.y - currentEnemy.transform.position.y) <= currentEnemy.attackRange)
        {
            //¹¥»÷
            isAttack = true;
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
            isAttack = false;
        }

        moveDir = (target - currentEnemy.transform.position).normalized;

        if (moveDir.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        if (moveDir.x < 0)
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isHurt && !currentEnemy.isDead && !isAttack)
        {
            currentEnemy.rb.velocity = currentEnemy.currentSpeed * Time.deltaTime * moveDir;
        }
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("chase", true);
    }
}
