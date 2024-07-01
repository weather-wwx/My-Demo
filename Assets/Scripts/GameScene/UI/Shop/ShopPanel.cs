using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum E_Shop_Type
{
    //所有
    All,
    //装备
    Equip,
    //道具
    Prop,
    //消耗品
    Consumables
}

public class ShopPanel : BasePanel
{
    private List<ShopCell> shopCells = new List<ShopCell>();
    private Transform content;

    protected override void Awake()
    {
        base.Awake();
        content = GetControl<ScrollRect>("Scroll View").content;
    }

    public override void Init()
    {
        ChangeType(E_Shop_Type.All);
        UpdateMoney();

        //关闭界面
        GetControl<Button>("btnClose").onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ShopPanel>("ShopPanel", false);
        });

        GetControl<Toggle>("toggle1").onValueChanged.AddListener(ToggelOnValueChanged);
        GetControl<Toggle>("toggle2").onValueChanged.AddListener(ToggelOnValueChanged);
        GetControl<Toggle>("toggle3").onValueChanged.AddListener(ToggelOnValueChanged);
        GetControl<Toggle>("toggle4").onValueChanged.AddListener(ToggelOnValueChanged);
    }

    private void ToggelOnValueChanged(bool value)
    {
        if (GetControl<Toggle>("toggle1").isOn)
        {
            ChangeType(E_Shop_Type.All);
        }
        else if (GetControl<Toggle>("toggle2").isOn)
        {
            ChangeType(E_Shop_Type.Equip);
        }
        else if (GetControl<Toggle>("toggle3").isOn)
        {
            ChangeType(E_Shop_Type.Prop);
        }
        else
        {
            ChangeType(E_Shop_Type.Consumables);
        }
    }

    public void ChangeType(E_Shop_Type type)
    {
        List<ItemInfo> list = new List<ItemInfo>();
        switch(type)
        {
            case E_Shop_Type.All:
                list.Clear();
                list = GameDataMgr.Instance.ShopData.allData;
                break;
            case E_Shop_Type.Equip:
                list.Clear();
                list = GameDataMgr.Instance.ShopData.equipData;
                break;
            case E_Shop_Type.Prop:
                list.Clear();
                list = GameDataMgr.Instance.ShopData.propData;
                break;
            case E_Shop_Type.Consumables:
                list.Clear();
                list = GameDataMgr.Instance.ShopData.consumablesData;
                break;
        }

        //清理之前的格子
        for (int i = 0; i < shopCells.Count; i++)
        {
            Destroy(shopCells[i].gameObject);
        }
        shopCells.Clear();

        //添加新的商品格子
        for (int i = 0; i < list.Count; i++)
        {
            ShopCell cell = Instantiate(Resources.Load<GameObject>("UI/ShopCell")).GetComponent<ShopCell>();
            cell.transform.SetParent(content, false);
            cell.InitInfo(list[i]);
            shopCells.Add(cell);
        }
    }

    public void UpdateMoney()
    {
        GetControl<Text>("txtMoney").text = GameDataMgr.Instance.PlayerData.money.ToString();
    }
}
