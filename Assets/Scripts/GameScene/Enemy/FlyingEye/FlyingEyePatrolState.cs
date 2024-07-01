using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyePatrolState : BaseState
{
    private Vector3 target;
    private Vector3 moveDir;

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        target = enemy.GetNewPoint();
    }

    public override void LogicUptate()
    {
        if(currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NpcState.Chase);
            return;
        }

        if(Mathf.Abs(target.x - currentEnemy.transform.position.x) <0.1f && Mathf.Abs(target.y -currentEnemy.transform.position.y) < 0.1f)
        {
            currentEnemy.wait = true;
            target = currentEnemy.GetNewPoint();
        }

        moveDir = (target - currentEnemy.transform.position).normalized;

        if(moveDir.x > 0)
        {
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public override void PhysicsUpdate()
    {
        if(!currentEnemy.wait && !currentEnemy.isHurt && !currentEnemy.isDead)
        {
            currentEnemy.rb.velocity = currentEnemy.currentSpeed * Time.deltaTime* moveDir;
        }
        else
        {
            currentEnemy.rb.velocity = Vector2.zero;
        }
    }

    public override void OnExit()
    {
        
    }
}
