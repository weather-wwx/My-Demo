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

    //所有游戏物品的信息
    private List<ObjInfo> objData;
    public List<ObjInfo> ObjData => objData;

    private ShopData shopData;
    public ShopData ShopData => shopData;

    private PlayerData playerData;
    public PlayerData PlayerData => playerData;

    public GameDataMgr()
    {
        //读取音乐存储数据
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");

        //读取excel表中 配置好的物品信息
        objData = JsonMgr.Instance.LoadData<List<ObjInfo>>("ObjData");

        shopData = JsonMgr.Instance.LoadData<ShopData>("ShopData");

        //读取人物信息
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");

        //第一次游戏时，保持游戏音乐和音效的开启
        if (PlayerData.isFristPlay == 1)
        {
            musicData.isOpenMusic = true;
            musicData.isOpenSound = true;
            musicData.musicVolume = 1;
            musicData.soundVolume = 1;
        }
    }

    #region 音乐和音效相关的设置
    //给外部提供保存音乐相关数据设置的方法
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }

    //设置音乐音量
    public void SetMusicValue(float value)
    {
        musicData.musicVolume = value;
        MusicController.Instance.SetBgMusicValue(value);
    }
    //设置音效音量
    public void SetSoundValue(float value)
    {
        musicData.soundVolume = value;
    }
    //设置音乐的开启
    public void SetMusicIsOpne(bool isOpne)
    {
        musicData.isOpenMusic = isOpne;
        MusicController.Instance.SetIsOpenBgMusic(isOpne);
    }
    //设置音效的开启
    public void SetSoundIsOpen(bool isOpen)
    {
        musicData.isOpenSound = isOpen;
    }
    #endregion

    //得到物品信息
    public ObjInfo GetInfo(int index)
    {
        if(index <= objData.Count)
        {
            return objData[index-1];
        }

        return null;
    }

    //添加物品
    public void AddObjItem(ObjInfo info, int amount)
    {
        //得到类型
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


    //重置数据
    public void NewPlayerData()
    {
        playerData = new PlayerData();
    }
}
