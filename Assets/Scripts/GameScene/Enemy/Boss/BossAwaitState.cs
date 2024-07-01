using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAwaitState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
    }

    public override void LogicUptate()
    {
        if(currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NpcState.Chase);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}
