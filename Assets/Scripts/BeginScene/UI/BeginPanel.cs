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
            //�����ؿ�ʼ���
            UIManager.Instance.HidePanel<BeginPanel>("BeginPanel", true);

            //��������
            GameDataMgr.Instance.NewPlayerData();
            QuestManager.Instance.NewQuestData();

            //�л�����,���뵭����ʵ��
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
            //��ȡ��Ϸ�浵�����ݴ浵���ݣ����ص�ͼ
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
            //���������
            UIManager.Instance.ShowPanel<SettingPanel>("SettingPanel", false);
        });

        GetControl<Button>("btnQuit").onClick.AddListener(() =>
        {
            //�˳���Ϸ
            Application.Quit();
        });
    }

    private void UpdatePanelInfo()
    {
        //������Ϸ�浵��Ϣ���������
        //�ı�Start��ť��Text���ָ�Continue��ť��ʹ��
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
