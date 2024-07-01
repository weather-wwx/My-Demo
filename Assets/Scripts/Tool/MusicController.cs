using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private static MusicController instance;
    public static MusicController Instance => instance;

    public AudioSource bgMusic;
    public AudioSource soundAudioSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        bgMusic.volume = GameDataMgr.Instance.MusicData.musicVolume;
        bgMusic.mute = !GameDataMgr.Instance.MusicData.isOpenMusic;
    }

    //�л���������
    public void ChangeBg(AudioClip clip)
    {
        bgMusic.clip = clip;
        bgMusic.Play();
    }

    //���ñ�����������
    public void SetBgMusicValue(float value)
    {
        bgMusic.volume = value;
    }
    //���ñ������ֿ���״̬
    public void SetIsOpenBgMusic(bool isOn)
    {
        bgMusic.mute = !isOn;
    }

    //��������run��Ч
    public void PlaySound(AudioClip clip)
    {
        if (soundAudioSource.isPlaying)
            return;
        soundAudioSource.clip = clip;
        soundAudioSource.volume = GameDataMgr.Instance.MusicData.soundVolume;
        soundAudioSource.mute = !GameDataMgr.Instance.MusicData.isOpenSound;
        soundAudioSource.Play();
    }

    //������Ч
    public void PlaySFX(AudioClip clip)
    {
        soundAudioSource.clip = clip;
        soundAudioSource.volume = GameDataMgr.Instance.MusicData.soundVolume;
        soundAudioSource.mute = !GameDataMgr.Instance.MusicData.isOpenSound;
        soundAudioSource.Play();
    }
}
