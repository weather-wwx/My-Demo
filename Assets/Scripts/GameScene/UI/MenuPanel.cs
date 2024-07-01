using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPanel : BasePanel
{
    public GameScene_So targetScene;

    public override void Init()
    {
        UpdatePanelInfo();

        GetControl<Button>("btnClose").onClick.AddListener(() =>
        {
            Time.timeScale = 1.0f;
            //隐藏自己
            UIManager.Instance.HidePanel<SettingPanel>("MenuPanel", false);
        });

        //回到主菜单
        GetControl<Button>("btnHome").onClick.AddListener(() =>
        {
            //恢复时间
            Time.timeScale = 1.0f;
            //切换到开始界面，保存游戏数据
            GameDataMgr.Instance.PlayerData.sceneName = SceneManager.GetActiveScene().name;
            GameDataMgr.Instance.PlayerData.pos = CameraControl.Instance.player.position.x.ToString() + "," +CameraControl.Instance.player.position.y.ToString() + "," + CameraControl.Instance.player.position.z.ToString();
            GameDataMgr.Instance.PlayerData.scale = (int)CameraControl.Instance.player.localScale.x;
            GameDataMgr.Instance.SavePlayData();
            GameDataMgr.Instance.SaveMusicData();
            QuestManager.Instance.SaveTaskData();
            UIManager.Instance.HidePanel<MenuPanel>("MenuPanel", false);
            UIManager.Instance.HidePanel<GamePanel>("GamePanel", false);
            SceneMgr.Instance.LoadScene(targetScene,()=>
            {
                MusicController.Instance.ChangeBg(Resources.Load<AudioClip>("Music/Bg/Bg1"));
            });
        });

        //退出游戏
        GetControl<Button>("btnQuit").onClick.AddListener(() =>
        {
            Time.timeScale = 1.0f;
            GameDataMgr.Instance.PlayerData.sceneName = SceneManager.GetActiveScene().name;
            GameDataMgr.Instance.PlayerData.pos = CameraControl.Instance.player.position.x.ToString() + CameraControl.Instance.player.position.y.ToString() + CameraControl.Instance.player.position.z.ToString();
            GameDataMgr.Instance.PlayerData.scale = (int)CameraControl.Instance.player.localScale.x;
            //保存游戏数据，退出游戏
            GameDataMgr.Instance.SavePlayData();
            GameDataMgr.Instance.SaveMusicData();
            QuestManager.Instance.SaveTaskData();
            Application.Quit();
        });


        GetControl<Slider>("sliderMusic").onValueChanged.AddListener((value) =>
        {
            GameDataMgr.Instance.SetMusicValue(value);
            GameDataMgr.Instance.SaveMusicData();
        });

        GetControl<Slider>("sliderSound").onValueChanged.AddListener((value) =>
        {
            GameDataMgr.Instance.SetSoundValue(value);
            GameDataMgr.Instance.SaveMusicData();
        });

        GetControl<Toggle>("toggleMusic").onValueChanged.AddListener((isOpen) =>
        {
            GameDataMgr.Instance.SetMusicIsOpne(isOpen);
            GameDataMgr.Instance.SaveMusicData();
        });

        GetControl<Toggle>("toggleSound").onValueChanged.AddListener((isOpen) =>
        {
            GameDataMgr.Instance.SetSoundIsOpen(isOpen);
            GameDataMgr.Instance.SaveMusicData();
        });
    }

    //更新面板内的数据
    public void UpdatePanelInfo()
    {
        //得到设置面板里的数据
        MusicData data = GameDataMgr.Instance.MusicData;

        GetControl<Slider>("sliderMusic").value = data.musicVolume;
        GetControl<Slider>("sliderSound").value = data.soundVolume;
        GetControl<Toggle>("toggleMusic").isOn = data.isOpenMusic;
        GetControl<Toggle>("toggleSound").isOn = data.isOpenSound;
    }
}
