using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : MonoBehaviour
{
    private static BossHealthBar instance;
    public static BossHealthBar Instance=>instance;

    private void Awake()
    {
        instance = this;
    }

    public RectTransform healthBar;

    public void ChangeHp(int maxHp,int hp)
    {
        float hpValue = (float)hp / maxHp;

        DOTween.To(() => healthBar.sizeDelta, x => healthBar.sizeDelta = x, new Vector2(hpValue * 900, 40), 0.5f);
    }
}
