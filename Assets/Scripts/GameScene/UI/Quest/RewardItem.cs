using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem : MonoBehaviour
{
    public Image imgIcon;
    public Text txtNum;

    public void SetupRewardPanel(ItemInfo info)
    {
        ObjInfo objInfo =GameDataMgr.Instance.GetInfo(info.id);

        //º”‘ÿÕº∆¨
        if (objInfo.type == 3)
        {
            imgIcon.sprite = Resources.Load<Sprite>(objInfo.icon);
        }
        else if(objInfo.type == 5)
        {
            imgIcon.sprite = MultipleMgr.Instatnce.GetSprite("Coin", objInfo.icon);
        }
        else
        {
            imgIcon.sprite = MultipleMgr.Instatnce.GetSprite("Icons", objInfo.icon);
        }

        txtNum.text = "°¡" + info.amount.ToString();
    }
}
