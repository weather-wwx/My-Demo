using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    //是否是第一次玩
    public int isFristPlay = 1;

    //背包数据信息
    public List<ItemInfo> equipData = new List<ItemInfo>();
    public List<ItemInfo> propData = new List<ItemInfo>();
    public List<ItemInfo> consumablesData = new List<ItemInfo>();

    //基础信息
    public int money = 1500;
    public int maxHp = 300;
    public int nowHp = 300;
    public int baseAttack = 20;
    public int baseDefense = 20;

    //当前装备的武器
    public ObjInfo currentWeapon;
    //当前装备的防具
    public ObjInfo currentArmor;

    public string pos;
    public int scale;
    public string sceneName;
}
