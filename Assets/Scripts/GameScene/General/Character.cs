using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    //最大血量
    public int maxHp;
    //当前血量
    public int nowHp;
    //基础攻击力
    public int baseAttack;
    //当前攻击
    public int nowAttack;

    //防御
    public int baseDefense;
    public int nowDefense;

    //无敌帧处理相关参数
    public float invulnerableDuration;
    public float invulnerableCounter;
    public bool inVulnerable;

    //韧性参数相关
    //韧性值
    public int resilienceCount;
    //当前韧性值
    public int nowResilience;
    //击退
    public bool isRepel;
    //攻击削韧
    public int attackResilience;

    //死亡
    public bool isDead;

    //攻击
    public abstract void TakeDamage(Character attacker);

    protected virtual void Update()
    {
        //处在无敌帧状态，退出伤害计算
        if (inVulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if(invulnerableCounter <= 0) 
            {
                inVulnerable = false;
            }
        }
    }

    //进入短暂的无敌状态
    protected void TriggerInvulnerable()
    {
        if (!inVulnerable)
        {
            inVulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}
