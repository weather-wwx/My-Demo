using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;

        currentEnemy.anim.SetBool("isWalk", true);
    }

    public override void LogicUptate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NpcState.Chase);
            return;
        }

        if (!currentEnemy.check.isGround || (currentEnemy.check.TouchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.check.TouchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool("isWalk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("isWalk", true);
        }
    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isHurt && !currentEnemy.isDead)
        {
            currentEnemy.rb.velocity = new Vector2(currentEnemy.currentSpeed * currentEnemy.faceDir.x * Time.deltaTime, 0);
        }
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("isWalk", false);
    }
}
