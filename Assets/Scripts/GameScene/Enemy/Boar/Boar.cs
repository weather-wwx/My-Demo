using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new BoarPatrolState();
        chaseState = new BoarChaseState();
    }

    private void Start()
    {
        currentSpeed = normalSpeed;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Player>()?.TakeDamage(this);
    }
}
