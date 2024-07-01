using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public RectTransform hp1Rect;
    public RectTransform hp2Rect;
    public RectTransform hp3Rect;

    private float slideCd;
    public float slideCounter;

    protected override void Start()
    {
        base.Start();

        slideCd = 3;
        slideCounter = 3;
    }

    public override void Init()
    {
        ChangeHp(GameDataMgr.Instance.PlayerData.maxHp, GameDataMgr.Instance.PlayerData.nowHp);

        GetControl<Button>("btnBag").onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<BasePanel>("BagPanel", false);
        });

        GetControl<Button>("btnMenu").onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<MenuPanel>("MenuPanel", false);
            Time.timeScale = 0;
        });

        GetControl<Button>("btnQuest").onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<QuestPanel>("QuestPanel", false);
            Time.timeScale = 0;
        });
    }

    protected override void Update()
    {
        base.Update();

        GetControl<Image>("imgSlide").fillAmount = slideCounter / slideCd;
        slideCounter += Time.deltaTime;
        if (slideCounter >= slideCd) 
        { 
            slideCounter = slideCd;
        }
    }

    public void ChangeHp(int maxHp, int hp)
    {
        float hpValue = (float)hp / maxHp;

        if (hpValue > 0.9f)
        {
            DOTween.To(() => hp3Rect.sizeDelta, x => hp3Rect.sizeDelta = x, new Vector2(hpValue * 500 - 450, 80), 0.5f);
            GetControl<Image>("hp2").GetComponent<RectTransform>().sizeDelta = new Vector2(400, 80);
            GetControl<Image>("hp1").GetComponent<RectTransform>().sizeDelta = new Vector2(50, 80);
        }
        else if (hpValue >= 0.1f && hpValue <= 0.9f)
        {
            GetControl<Image>("hp3").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 80);
            DOTween.To(() => hp2Rect.sizeDelta, x => hp2Rect.sizeDelta = x, new Vector2(hpValue * 500 - 50, 80), 0.5f);
            GetControl<Image>("hp1").GetComponent<RectTransform>().sizeDelta = new Vector2(50, 80);
        }
        else
        {
            GetControl<Image>("hp3").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 80);
            GetControl<Image>("hp2").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 80);
            DOTween.To(() => hp1Rect.sizeDelta, x => hp1Rect.sizeDelta = x, new Vector2(hpValue * 500, 80), 0.5f);
        }
    }
}
