using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCell : BasePanel
{
    private ObjInfo objInfo;

    public override void Init()
    {
        isShow = true;

        GetControl<Button>("btnBuy").onClick.AddListener(() =>
        {
            //购买后，添加背包，更新数据
            //判断能否购买
            if (GameDataMgr.Instance.PlayerData.money < objInfo.price)
                return;

            GameDataMgr.Instance.PlayerData.money -= objInfo.price;
            UIManager.Instance.GetPanel<ShopPanel>("ShopPanel").UpdateMoney();
            GameDataMgr.Instance.AddObjItem(objInfo, 1);

            //购物任务检测
            QuestManager.Instance.UpdateQuestProgress(objInfo.name, 1);
        });
    }

    public void InitInfo(ItemInfo info)
    {
        objInfo = GameDataMgr.Instance.GetInfo(info.id);
        //加载图片
        if (objInfo.type == 3)
        {
            GetControl<Image>("imgIcon").sprite = Resources.Load<Sprite>(objInfo.icon);
        }
        else
        {
            GetControl<Image>("imgIcon").sprite = MultipleMgr.Instatnce.GetSprite("Icons", objInfo.icon);
        }

        GetControl<Text>("txtName").text = objInfo.name;
        GetControl<Text>("txtPrice").text = objInfo.price.ToString();
        GetControl<Text>("txtEffect").text = objInfo.effect;
    }
}
