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
        //根据道具信息的数据 来更新格子对象
        ObjInfo itemData = GameDataMgr.Instance.GetInfo(info.id);
        //使用我们的道具表中的数据
        //名字
        GetControl<Text>("txtName").text = itemData.name;
        //效果
        GetControl<Text>("txtEffect").text = "效果:" + itemData.effect;
        //描述
        GetControl<Text>("txtTip").text = "描述:" + itemData.tips;
    }
}
