using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    //´ý»ú×´Ì¬
    protected BaseState awaitState;

    public GameObject portal;
    private GameObject spell;
    private GameObject fireBall;
    public List<Transform> shootPos = new List<Transform>();
    private float shootCounter;
    private float shootTime;
    private int shootNum;

    private bool firstFire;

    protected override void OnEnable()
    {
        currentState = awaitState;
        currentState.OnEnter(this);
    }

    protected override void Awake()
    {
        base.Awake();

        awaitState = new BossAwaitState();
        patrolState = new BossPatrolState();
        chaseState = new BossChaseState();

        spell = Resources.Load<GameObject>("Prefabs/Spell");
        fireBall = Resources.Load<GameObject>("Prefabs/Fireball");

        shootCounter = 0;
        shootTime = 2f;
    }

    protected override void Update()
    {
        base.Update();

        if(shootNum > 4)
            firstFire = true;

        shootCounter -= Time.deltaTime;

        if (nowHp <= maxHp/2 && shootNum<5 && !firstFire)
        {
            HalfHpEvent();
        }

        if(nowHp < maxHp / 10)
        {
            nowAttack = baseAttack + 10;
            nowDefense = baseDefense + 5;
            CrueltyEvent();
        }
    }

    public override void TakeDamage(Character attacker)
    {
        //´¦ÔÚÎÞµÐÖ¡×´Ì¬£¬ÍË³öÉËº¦¼ÆËã
        if (inVulnerable)
            return;

        int damage = attacker.nowAttack - this.nowDefense;
        if ((nowHp - damage) > 0)
        {
            nowHp -= damage;
            BossHealthBar.Instance.ChangeHp(maxHp, nowHp);
            TriggerInvulnerable();

            //¹ÖÎïÑªÌõÉÁË¸
            OnTakeDamage(attacker.transform);

            //ÈÍÐÔÅÐ¶Ï
            nowResilience -= attacker.attackResilience;
            if (nowResilience <= 0)
            {
                isRepel = true;
                //Ï÷³ýÈÍÐÔ£¬Ôì³ÉÊÜÉË»÷ÍË
                OnRepel();
            }
        }
        else
        {
            nowHp = 0;
            BossHealthBar.Instance.ChangeHp(maxHp, nowHp);
            //´¥·¢ËÀÍö£¬½øÐÐËÀÍöÂß¼­
            OnDie();
        }
    }

    public override void Move()
    {
    }

    public override bool FoundPlayer()
    {
        var obj = Physics2D.BoxCast(transform.position + (Vector3)currentOffset, checkSize, 0, faceDir, checkDistacne,
                                 1 << LayerMask.NameToLayer("Player"));
        if (obj)
        {
            attacker = obj.transform;
        }

        return obj;
    }

    public void CastAttack()
    {
        Spell obj = Instantiate(spell, new Vector3(attacker.position.x, -1.12f, 0), Quaternion.identity).GetComponent<Spell>();
        obj.GetLauncher(this);
    }

    public void HalfHpEvent()
    {
        if (shootCounter <= 0)
        {
            Fireball ball = Instantiate(fireBall, shootPos[shootNum]).GetComponent<Fireball>();
            ball.GetLauncher(this);
            shootNum++;
            shootCounter = shootTime;
        }
    }

    public void CrueltyEvent()
    {
        if (shootCounter <= 0)
        {
            shootCounter = shootTime + 3;

            for (int i = 0; i < shootPos.Count; i++)
            {
                Fireball ball = Instantiate(fireBall, shootPos[i]).GetComponent<Fireball>();
                ball.GetLauncher(this);
            }
        }
    }

    public override void DestroyAfterAnimation()
    {
        portal.SetActive(true);
        Destroy(this.gameObject);
    }
}
