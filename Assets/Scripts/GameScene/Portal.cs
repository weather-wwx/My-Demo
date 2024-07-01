using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private float timer;
    private float portalTime;
    private bool isStart;

    //传送的位置
    public Vector3 targetPos;
    public Vector3 scale;
    //目标场景
    public GameScene_So targetScene;
    //场景音乐
    public AudioClip clip;

    private void Start()
    {
        portalTime = 3f;
    }

    private void Update()
    {
        if(isStart)
        {
            timer += Time.deltaTime;
            if(timer >= portalTime)
            {
                timer = portalTime;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isStart = true;
            //进行传送
            if(timer == portalTime)
            {
                UIManager.Instance.HidePanel<GamePanel>("GamePanel", false);
                SceneMgr.Instance.LoadScene(targetScene,()=>
                {
                    CameraControl.Instance.SetLookAt(targetPos, scale);
                    MusicController.Instance.ChangeBg(clip);
                    UIManager.Instance.ShowPanel<GamePanel>("GamePanel", false);
                });

                timer = 0;
                isStart = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        timer = 0;
        isStart = false;
    }
}
