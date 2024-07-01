using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum E_NowAttackSate
{
    Attack1,
    Attack2,
    Attack3,
    AirAttack1,
    AirAttack2,
    AirAttack3,
}

public class Player : Character
{
    private static Player instance;
    public static Player Instance=>instance;

    public E_NowAttackSate nowAttackSate;

    public UnityEvent OnTakeDamage;
    public UnityEvent<Transform> OnTakeRepel;
    public UnityEvent OnDie;

    //当前装备的武器
    private ObjInfo currentWeapon;
    //当前装备的防具
    private ObjInfo currentArmor;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        nowAttackSate = E_NowAttackSate.Attack1;
        maxHp = GameDataMgr.Instance.PlayerData.maxHp;
        nowHp = GameDataMgr.Instance.PlayerData.nowHp;
        baseAttack = GameDataMgr.Instance.PlayerData.baseAttack;
        baseDefense = GameDataMgr.Instance.PlayerData.baseDefense;

        if(GameDataMgr.Instance.PlayerData.currentWeapon != null)
        {
            currentWeapon = GameDataMgr.Instance.PlayerData.currentWeapon;
            nowAttack = baseAttack + currentWeapon.effectValue;
        }
        else
        {
            nowAttack = baseAttack;
        }

        if (GameDataMgr.Instance.PlayerData.currentArmor != null)
        {
            currentArmor = GameDataMgr.Instance.PlayerData.currentArmor;
            nowDefense = baseDefense + currentArmor.effectValue;
        }
        else
        {
            nowDefense = baseDefense;
        }
    }

    protected override void Update()
    {
        base.Update();
        
    }

    public override void TakeDamage(Character attacker)
    {
        //处在无敌帧状态，退出伤害计算
        if (inVulnerable)
            return;

        int damage = attacker.nowAttack - this.nowDefense;

        if (damage < 0)
        {
            damage = 5;
        }

        if ((nowHp - damage) >0)
        {
            nowHp -= damage;
            GameDataMgr.Instance.PlayerData.nowHp = nowHp;
            UIManager.Instance.GetPanel<GamePanel>("GamePanel").ChangeHp(maxHp, nowHp);
            TriggerInvulnerable();

            //人物血条闪烁
            OnTakeDamage?.Invoke();
            //韧性判断
            nowResilience -= attacker.attackResilience;
            if(nowResilience <= 0 )
            {
                isRepel = true;
                //削除韧性，造成受伤击退和打断
                OnTakeRepel?.Invoke(attacker.transform);
            }
        }
        else
        {
            nowHp = 0;
            UIManager.Instance.GetPanel<GamePanel>("GamePanel").ChangeHp(maxHp, nowHp);
            //触发死亡，进行死亡逻辑
            OnDie?.Invoke();
        }
    }


    public void EquipWeapon()
    {
        currentWeapon = GameDataMgr.Instance.PlayerData.currentWeapon;
        nowAttack = baseAttack + currentWeapon.effectValue;
    }

    public void EquipArmor()
    {
        currentArmor = GameDataMgr.Instance.PlayerData.currentArmor;
        nowDefense = baseDefense + currentArmor.effectValue;
    }

    public void AddAttackValue(int value)
    {
        baseAttack += value;
        nowAttack += value; 
    }

    public void AddDefenseValue(int value)
    {
        baseDefense += value;
        nowDefense += value;
    }
}
