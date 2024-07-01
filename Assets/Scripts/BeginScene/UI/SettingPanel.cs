using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public override void Init()
    {
        UpdatePanelInfo();

        GetControl<Button>("btnClose").onClick.AddListener(() =>
        {
            //隐藏自己
            UIManager.Instance.HidePanel<SettingPanel>("SettingPanel", false);
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
