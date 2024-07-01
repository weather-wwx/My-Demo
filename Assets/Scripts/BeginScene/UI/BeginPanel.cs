using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public GameScene_So targetScene;

    public override void Init()
    {
        UpdatePanelInfo();

        GetControl<Button>("btnStart").onClick.AddListener(() =>
        {
            //逐渐隐藏开始面板
            UIManager.Instance.HidePanel<BeginPanel>("BeginPanel", true);

            //重置数据
            GameDataMgr.Instance.NewPlayerData();
            QuestManager.Instance.NewQuestData();

            //切换场景,淡入淡出的实现
            SceneMgr.Instance.LoadScene(targetScene, () =>
            {
                GameDataMgr.Instance.PlayerData.isFristPlay = 0;
                UIManager.Instance.ShowPanel<GamePanel>("GamePanel", false);
                MusicController.Instance.ChangeBg(Resources.Load<AudioClip>("Music/Bg/Bg2"));
                CameraControl.Instance.SetLookAt(new Vector3(3.5f, -3f, 0), Vector3.one);
            });
            
        });

        GetControl<Button>("btnContinue").onClick.AddListener(() =>
        {
            //读取游戏存档，根据存档内容，加载地图
            UIManager.Instance.HidePanel<BeginPanel>("BeginPanel", true);
            SceneMgr.Instance.LoadScene(SceneMgr.Instance.GetScene(GameDataMgr.Instance.PlayerData.sceneName), () =>
            {
                UIManager.Instance.ShowPanel<GamePanel>("GamePanel", false);
                MusicController.Instance.ChangeBg(Resources.Load<AudioClip>("Music/Bg/Bg2"));
                string[] xyz = GameDataMgr.Instance.PlayerData.pos.Split(',');
                CameraControl.Instance.SetLookAt(new Vector3(float.Parse(xyz[0]), float.Parse(xyz[1]), float.Parse(xyz[2])), 
                                                 new Vector3(GameDataMgr.Instance.PlayerData.scale, 1, 1));
            });
        });

        GetControl<Button>("btnSetting").onClick.AddListener(() =>
        {
            //打开设置面板
            UIManager.Instance.ShowPanel<SettingPanel>("SettingPanel", false);
        });

        GetControl<Button>("btnQuit").onClick.AddListener(() =>
        {
            //退出游戏
            Application.Quit();
        });
    }

    private void UpdatePanelInfo()
    {
        //根据游戏存档信息，更新面板
        //改变Start按钮的Text，恢复Continue按钮的使用
        if(GameDataMgr.Instance.PlayerData.isFristPlay == 1)
        {
            GetControl<Button>("btnContinue").enabled = false;
        }
        else
        {
            GetControl<Button>("btnContinue").enabled = true;
            GetControl<Text>("txtContinue").color = Color.white;
        }
    }
}
