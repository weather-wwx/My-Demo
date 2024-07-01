using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : Enemy
{
    //Ñ²Âß·¶Î§
    public float patrolRadius;
    //³öÉúµã
    public Vector3 spwanPoint;

    protected override void Awake()
    {
        base.Awake();

        spwanPoint = transform.position;
        patrolState = new FlyingEyePatrolState();
        chaseState = new FlyingEyeChaseState();
    }

    public override void Move()
    {
    }

    public override bool FoundPlayer()
    {
        var obj = Physics2D.OverlapCircle(transform.position, checkDistacne, 1 << LayerMask.NameToLayer("Player"));
        if(obj)
        {
            attacker = obj.transform;
        }

        return obj;
    }

    public override Vector3 GetNewPoint()
    {
        var targetX = Random.Range(-patrolRadius, patrolRadius);
        var targetY = Random.Range(-patrolRadius, patrolRadius);

        return spwanPoint + new Vector3 (targetX, targetY, 0);
    }

    public override void OnDie()
    {
        base.OnDie();
        rb.gravityScale = 4;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, checkDistacne);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spwanPoint, patrolRadius);
    }
}
