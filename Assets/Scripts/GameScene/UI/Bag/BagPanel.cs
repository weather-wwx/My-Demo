using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//背包的页签
public enum E_Bag_Type
{
    //装备
    Equip,
    //道具
    Prop,
    //消耗品
    Consumables
}

public class BagPanel : BasePanel
{
    public List<ItemCell> itemCells = new List<ItemCell>();
    public E_Bag_Type currentType;

    public override void Init()
    {
        ChangeType(E_Bag_Type.Equip);
        UpdateEquip();
        GetControl<Text>("txtMoney").text = GameDataMgr.Instance.PlayerData.money.ToString();

        GetControl<Button>("btnClose").onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<BagPanel>("BagPanel", false);
        });

        GetControl<Toggle>("toggle1").onValueChanged.AddListener(ToggelOnValueChanged);
        GetControl<Toggle>("toggle2").onValueChanged.AddListener(ToggelOnValueChanged);
        GetControl<Toggle>("toggle3").onValueChanged.AddListener(ToggelOnValueChanged);
    }

    private void ToggelOnValueChanged(bool value)
    {
        if (GetControl<Toggle>("toggle1").isOn)
        {
            ChangeType(E_Bag_Type.Equip);
        }
        else if (GetControl<Toggle>("toggle2").isOn)
        {
            ChangeType(E_Bag_Type.Prop);
        }
        else
        {
            ChangeType(E_Bag_Type.Consumables);
        }
    }

    //背包的页签切换
    public void ChangeType(E_Bag_Type type)
    {
        currentType = type;

        List<ItemInfo> list = new List<ItemInfo>();
        switch (type)
        {
            case E_Bag_Type.Equip:
                list.Clear();
                list = GameDataMgr.Instance.PlayerData.equipData;
                break;
            case E_Bag_Type.Prop:
                list.Clear();
                list = GameDataMgr.Instance.PlayerData.propData;
                break;
            case E_Bag_Type.Consumables:
                list.Clear();
                list = GameDataMgr.Instance.PlayerData.consumablesData;
                break;
        }

        //隐藏之前的内容
        for(int i = 0; i < itemCells.Count; i++)
        {
            itemCells[i].infoObj.SetActive(false);
        }

        if (list != null)
        {
            //更新新的内容
            for (int i = 0; i < list.Count; i++)
            {
                itemCells[i].InitInfo(list[i]);
            }
        }
    }

    public void UpdateEquip()
    {
        if(GameDataMgr.Instance.PlayerData.currentWeapon == null)
            GetControl<Image>("imgWeapon").gameObject.SetActive(false);
        else
        {
            GetControl<Image>("imgWeapon").gameObject.SetActive(true);
            GetControl<Image>("imgWeapon").sprite = MultipleMgr.Instatnce.GetSprite("Icons", GameDataMgr.Instance.PlayerData.currentWeapon.icon);
        }

        if(GameDataMgr.Instance.PlayerData.currentArmor == null)
            GetControl<Image>("imgArmor").gameObject.SetActive(false);
        else
        {
            GetControl<Image>("imgArmor").gameObject.SetActive(true);
            GetControl<Image>("imgArmor").sprite = MultipleMgr.Instatnce.GetSprite("Icons", GameDataMgr.Instance.PlayerData.currentArmor.icon);
        }
    }
}
