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

        //����������������Ƴ����¼�
        EventTrigger trigger = GetControl<Image>("imgIcon").gameObject.AddComponent<EventTrigger>();

        //����һ�� ��������¼������
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener(EnterItemCell);

        //����һ�� ����Ƴ����¼������
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
        //��ʾ��ʾ���
        UIManager.Instance.ShowPanel<TipsPanel>("TipsPanel", false, (p) =>
        {
            //��ʼ����Ϣ
            p.InitInfo(itemInfo);
            //����λ��
            p.gameObject.transform.position = GetControl<Image>("imgIcon").transform.position;
        });
    }

    private void ExitItemCell(BaseEventData data)
    {
        //������ʾ���
        UIManager.Instance.HidePanel<TipsPanel>("TipsPanel", false);
    }

    //���ݵ�����Ϣ ���� ������Ϣ
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

