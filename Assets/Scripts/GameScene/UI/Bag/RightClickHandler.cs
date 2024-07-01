using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RightClickHandler : MonoBehaviour, IPointerClickHandler
{
    public ItemCell cell;
    private ObjInfo info;

    private void Update()
    {
        if (cell != null)
        {
            info = GameDataMgr.Instance.GetInfo(cell.itemInfo.id);
        }
    }

    //右键使用物品
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            int type = info.type;
            switch (type)
            {
                case 1:
                    GameDataMgr.Instance.PlayerData.currentWeapon = info;
                    Player.Instance.EquipWeapon();
                    UIManager.Instance.GetPanel<BagPanel>("BagPanel").UpdateEquip();
                    break;
                case 2:
                    GameDataMgr.Instance.PlayerData.currentArmor = info;
                    Player.Instance.EquipArmor();
                    UIManager.Instance.GetPanel<BagPanel>("BagPanel").UpdateEquip();
                    break;
                case 3:
                    UseProp(info.id);
                    break;
                case 4:
                    UseConsumables();
                    break;
            }
        }
    }

    //使用道具
    public void UseProp(int id)
    {
        switch (id)
        {
            case 9:
                GameDataMgr.Instance.PlayerData.baseAttack += info.effectValue;
                Player.Instance.AddAttackValue(info.effectValue);
                break;
            case 10:
                GameDataMgr.Instance.PlayerData.maxHp += info.effectValue;
                Player.Instance.maxHp = GameDataMgr.Instance.PlayerData.maxHp;
                UIManager.Instance.GetPanel<GamePanel>("GamePanel").ChangeHp(Player.Instance.maxHp, Player.Instance.nowHp);
                break;
            case 11:
                GameDataMgr.Instance.PlayerData.baseAttack += info.effectValue;
                Player.Instance.AddAttackValue(info.effectValue);
                break;
            case 12:
                GameDataMgr.Instance.PlayerData.baseDefense += info.effectValue;
                Player.Instance.AddDefenseValue(info.effectValue);
                break;
        }
        for (int i = 0; i < GameDataMgr.Instance.PlayerData.propData.Count; i++)
        {
            if (GameDataMgr.Instance.PlayerData.propData[i].id == info.id)
            {
                GameDataMgr.Instance.PlayerData.propData[i].amount--;
                cell.GetControl<Text>("txtNum").text = cell.itemInfo.amount.ToString();
                if (GameDataMgr.Instance.PlayerData.propData[i].amount == 0)
                {
                    GameDataMgr.Instance.PlayerData.propData.RemoveAt(i);
                    UIManager.Instance.HidePanel<TipsPanel>("TipsPanel", false);
                    UIManager.Instance.GetPanel<BagPanel>("BagPanel").ChangeType(UIManager.Instance.GetPanel<BagPanel>("BagPanel").currentType);
                }
            }
        }
    }

    //使用药品
    public void UseConsumables()
    {
        if (GameDataMgr.Instance.PlayerData.nowHp == GameDataMgr.Instance.PlayerData.maxHp)
            return;

        GameDataMgr.Instance.PlayerData.nowHp += info.effectValue;

        if (GameDataMgr.Instance.PlayerData.nowHp >= GameDataMgr.Instance.PlayerData.maxHp)
            GameDataMgr.Instance.PlayerData.nowHp = GameDataMgr.Instance.PlayerData.maxHp;

        Player.Instance.nowHp = GameDataMgr.Instance.PlayerData.nowHp;
        UIManager.Instance.GetPanel<GamePanel>("GamePanel").ChangeHp(Player.Instance.maxHp, Player.Instance.nowHp);

        for (int i = 0; i < GameDataMgr.Instance.PlayerData.consumablesData.Count; i++)
        {
            if (GameDataMgr.Instance.PlayerData.consumablesData[i].id == info.id)
            {
                GameDataMgr.Instance.PlayerData.consumablesData[i].amount--;
                cell.GetControl<Text>("txtNum").text = cell.itemInfo.amount.ToString();
                if (GameDataMgr.Instance.PlayerData.consumablesData[i].amount == 0)
                {
                    GameDataMgr.Instance.PlayerData.consumablesData.RemoveAt(i);
                    UIManager.Instance.HidePanel<TipsPanel>("TipsPanel", false);
                    UIManager.Instance.GetPanel<BagPanel>("BagPanel").ChangeType(UIManager.Instance.GetPanel<BagPanel>("BagPanel").currentType);
                }
            }
        }
    }
}
