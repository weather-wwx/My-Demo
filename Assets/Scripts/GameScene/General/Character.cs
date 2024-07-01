using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    //���Ѫ��
    public int maxHp;
    //��ǰѪ��
    public int nowHp;
    //����������
    public int baseAttack;
    //��ǰ����
    public int nowAttack;

    //����
    public int baseDefense;
    public int nowDefense;

    //�޵�֡������ز���
    public float invulnerableDuration;
    public float invulnerableCounter;
    public bool inVulnerable;

    //���Բ������
    //����ֵ
    public int resilienceCount;
    //��ǰ����ֵ
    public int nowResilience;
    //����
    public bool isRepel;
    //��������
    public int attackResilience;

    //����
    public bool isDead;

    //����
    public abstract void TakeDamage(Character attacker);

    protected virtual void Update()
    {
        //�����޵�֡״̬���˳��˺�����
        if (inVulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if(invulnerableCounter <= 0) 
            {
                inVulnerable = false;
            }
        }
    }

    //������ݵ��޵�״̬
    protected void TriggerInvulnerable()
    {
        if (!inVulnerable)
        {
            inVulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}
