using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.anim.SetBool("isRun", true);
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
    }

    public override void LogicUptate()
    {
        if(currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NpcState.Patrol);
        }

        if (!currentEnemy.check.isGround || (currentEnemy.check.TouchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.check.TouchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x * 1, 1, 1);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("isRun", false);
    }
}
