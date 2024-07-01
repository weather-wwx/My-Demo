using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCell : BasePanel
{
    [HideInInspector]
    public ItemInfo itemInfo;
    public GameObject infoObj;

    protected override void Start()
    {
        base.Start();

        //监听鼠标移入和鼠标移出的事件
        EventTrigger trigger = GetControl<Image>("imgIcon").gameObject.AddComponent<EventTrigger>();

        //申明一个 鼠标进入的事件类对象
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener(EnterItemCell);

        //申明一个 鼠标移除的事件类对象
        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener(ExitItemCell);

        trigger.triggers.Add(enter);
        trigger.triggers.Add(exit);
    }

    public override void Init()
    {
        isShow = true;
    }

    private void EnterItemCell(BaseEventData data)
    {
        //显示提示面板
        UIManager.Instance.ShowPanel<TipsPanel>("TipsPanel", false, (p) =>
        {
            //初始化信息
            p.InitInfo(itemInfo);
            //更新位置
            p.gameObject.transform.position = GetControl<Image>("imgIcon").transform.position;
        });
    }

    private void ExitItemCell(BaseEventData data)
    {
        //隐藏提示面板
        UIManager.Instance.HidePanel<TipsPanel>("TipsPanel", false);
    }

    //根据道具信息 更新 格子信息
    public void InitInfo(ItemInfo info)
    {
        this.itemInfo = info;

        ObjInfo objInfo = GameDataMgr.Instance.GetInfo(itemInfo.id);
        infoObj.SetActive(true);
        if(objInfo.type == 3)
        {
            GetControl<Image>("imgIcon").sprite = Resources.Load<Sprite>(objInfo.icon);
        }
        else
        {
            GetControl<Image>("imgIcon").sprite = MultipleMgr.Instatnce.GetSprite("Icons", objInfo.icon);
        }

        GetControl<Text>("txtNum").text = info.amount.ToString();
    }
}

