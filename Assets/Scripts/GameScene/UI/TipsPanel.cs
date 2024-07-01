using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanel : BasePanel
{
    public override void Init()
    {
        isShow = true;
    }

    public void InitInfo(ItemInfo info)
    {
        //���ݵ�����Ϣ������ �����¸��Ӷ���
        ObjInfo itemData = GameDataMgr.Instance.GetInfo(info.id);
        //ʹ�����ǵĵ��߱��е�����
        //����
        GetControl<Text>("txtName").text = itemData.name;
        //Ч��
        GetControl<Text>("txtEffect").text = "Ч��:" + itemData.effect;
        //����
        GetControl<Text>("txtTip").text = "����:" + itemData.tips;
    }
}
