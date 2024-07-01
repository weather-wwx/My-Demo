using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private float timer;
    private float portalTime;
    private bool isStart;

    //���͵�λ��
    public Vector3 targetPos;
    public Vector3 scale;
    //Ŀ�곡��
    public GameScene_So targetScene;
    //��������
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
            //���д���
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
