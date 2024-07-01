using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr
{
    private static GameDataMgr instance;
    public static GameDataMgr Instance
    {
        get
        { 
            if(instance == null)
            {
                instance = new GameDataMgr ();
            }
            return instance;
        }
    }

    private MusicData musicData;
    public MusicData MusicData => musicData;

    //������Ϸ��Ʒ����Ϣ
    private List<ObjInfo> objData;
    public List<ObjInfo> ObjData => objData;

    private ShopData shopData;
    public ShopData ShopData => shopData;

    private PlayerData playerData;
    public PlayerData PlayerData => playerData;

    public GameDataMgr()
    {
        //��ȡ���ִ洢����
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");

        //��ȡexcel���� ���úõ���Ʒ��Ϣ
        objData = JsonMgr.Instance.LoadData<List<ObjInfo>>("ObjData");

        shopData = JsonMgr.Instance.LoadData<ShopData>("ShopData");

        //��ȡ������Ϣ
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");

        //��һ����Ϸʱ��������Ϸ���ֺ���Ч�Ŀ���
        if (PlayerData.isFristPlay == 1)
        {
            musicData.isOpenMusic = true;
            musicData.isOpenSound = true;
            musicData.musicVolume = 1;
            musicData.soundVolume = 1;
        }
    }

    #region ���ֺ���Ч��ص�����
    //���ⲿ�ṩ������������������õķ���
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }

    //������������
    public void SetMusicValue(float value)
    {
        musicData.musicVolume = value;
        MusicController.Instance.SetBgMusicValue(value);
    }
    //������Ч����
    public void SetSoundValue(float value)
    {
        musicData.soundVolume = value;
    }
    //�������ֵĿ���
    public void SetMusicIsOpne(bool isOpne)
    {
        musicData.isOpenMusic = isOpne;
        MusicController.Instance.SetIsOpenBgMusic(isOpne);
    }
    //������Ч�Ŀ���
    public void SetSoundIsOpen(bool isOpen)
    {
        musicData.isOpenSound = isOpen;
    }
    #endregion

    //�õ���Ʒ��Ϣ
    public ObjInfo GetInfo(int index)
    {
        if(index <= objData.Count)
        {
            return objData[index-1];
        }

        return null;
    }

    //�����Ʒ
    public void AddObjItem(ObjInfo info, int amount)
    {
        //�õ�����
        int type = info.type;
        ItemInfo item = new ItemInfo() { id = info.id, amount = amount };

        switch (type)
        {
            case 1:
            case 2:
                playerData.equipData.Add(item);
                break;
            case 3:
                if(playerData.propData.Count == 0)
                {
                    playerData.propData.Add(item);
                }
                else
                {
                    for (int i = 0; i < playerData.propData.Count; i++)
                    {
                        if (playerData.propData[i].id == info.id)
                        {
                            playerData.propData[i].amount += amount;
                            return;
                        }
                    }
                    playerData.propData.Add(item);
                }
                break;
            case 4:
                if(playerData.consumablesData.Count == 0)
                {
                    playerData.consumablesData.Add(item);
                }
                else
                {
                    for (int i = 0; i < playerData.consumablesData.Count; i++)
                    {
                        if (playerData.consumablesData[i].id == info.id)
                        {
                            playerData.consumablesData[i].amount += amount;
                            return;
                        }
                    }
                    playerData.consumablesData.Add(item);
                }
                break;
            case 5:
                playerData.money += amount;
                break;
        }
    }

    public void SavePlayData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }


    //��������
    public void NewPlayerData()
    {
        playerData = new PlayerData();
    }
}
