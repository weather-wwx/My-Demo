using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    //�Ƿ��ǵ�һ����
    public int isFristPlay = 1;

    //����������Ϣ
    public List<ItemInfo> equipData = new List<ItemInfo>();
    public List<ItemInfo> propData = new List<ItemInfo>();
    public List<ItemInfo> consumablesData = new List<ItemInfo>();

    //������Ϣ
    public int money = 1500;
    public int maxHp = 300;
    public int nowHp = 300;
    public int baseAttack = 20;
    public int baseDefense = 20;

    //��ǰװ��������
    public ObjInfo currentWeapon;
    //��ǰװ���ķ���
    public ObjInfo currentArmor;

    public string pos;
    public int scale;
    public string sceneName;
}
