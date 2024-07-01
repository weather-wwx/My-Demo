using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
       currentEnemy = enemy;
       currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }

    public override void LogicUptate()
    {
        if(currentEnemy.FoundPlayer())
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
        
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("isWalk", false);
    }
}
