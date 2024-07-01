using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Enemy : Character
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public PhysicsCheck check;

    //正常的移动速度
    public float normalSpeed;
    //追击敌人的速度
    public float chaseSpeed;
    //当前的速度
    [HideInInspector]public float currentSpeed;

    public Vector3 faceDir;
    public float hurtForce;

    public bool wait;
    public bool isHurt;
    public float waitTime;
    public float waitTimeCounter;
    public float lostTime;
    public float lostTimeCounter;

    //攻击相关的参数
    //攻击范围
    public float attackRange;
    //攻击间隔
    public float attackRate;
    //攻击目标
    public Transform attacker;

    //敌人检测
    public Vector2 currentOffset;
    public Vector2 checkSize;
    public float checkDistacne;

    public bool isAttack;

    //当前状态
    protected BaseState currentState;
    //巡逻状态
    protected BaseState patrolState;
    //追击状态
    protected BaseState chaseState;

    protected virtual void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    protected virtual void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        check = this.GetComponent<PhysicsCheck>();
    }

    protected override void Update()
    {
        base.Update();

        if(!isRepel)
        {
            anim.SetBool("isRepel", isRepel);
        }

        faceDir = new Vector3(-transform.localScale.x, 0, 0).normalized;

        currentState.LogicUptate();
        TimeCounter();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();

        if (!isHurt && !isDead && !wait)
        {
            Move();
        }
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }

    public override void TakeDamage(Character attacker)
    {
        //处在无敌帧状态，退出伤害计算
        if (inVulnerable)
            return;

        int damage = attacker.nowAttack - this.nowDefense;
        if(damage < 0)
        {
            damage = 10;
        }

        if ((nowHp - damage) > 0)
        {
            nowHp -= damage;
            TriggerInvulnerable();

            //怪物血条闪烁
            OnTakeDamage(attacker.transform);

            //韧性判断
            nowResilience -= attacker.attackResilience;
            if (nowResilience <= 0)
            {
                isRepel = true;
                //削除韧性，造成受伤击退
                OnRepel();
            }
        }
        else
        {
            nowHp = 0;
            //触发死亡，进行死亡逻辑
            OnDie();
        }
    }

    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }

    public void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x * 1, 1, 1);
            }
        }

        if(!FoundPlayer() && lostTimeCounter > 0)
        {
            lostTimeCounter -= Time.deltaTime;
        }
        else if(FoundPlayer())
        {
            lostTimeCounter = lostTime;
        }
    }

    public virtual bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)currentOffset, checkSize, 0, faceDir, checkDistacne,
                                 1 << LayerMask.NameToLayer("Player"));
    }

    public void SwitchState(NpcState state)
    {
        var newState = state switch
        {
            NpcState.Patrol => patrolState,
            NpcState.Chase => chaseState,
            _ => null
        };

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    public virtual Vector3 GetNewPoint()
    {
        return this.transform.position;
    }

    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        anim.SetTrigger("hurt");

        //判断敌人位置， 进行转向
        if (attacker.transform.position.x - this.transform.position.x > 0)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attacker.transform.position.x - this.transform.position.x < 0)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void OnRepel()
    {
        isHurt = true;
        anim.SetBool("isRepel", isRepel);
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public virtual void OnDie()
    {
        gameObject.layer = 2;
        isDead = true;
        anim.SetBool("isDead", true);
        QuestManager.Instance.UpdateQuestProgress(gameObject.name, 1);
    }

    public virtual void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position + (Vector3)currentOffset + new Vector3(checkDistacne * -transform.localScale.x, 0, 0), 0.05f);
    }
}
