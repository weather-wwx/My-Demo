using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneFadeInOut : MonoBehaviour
{
    private static SceneFadeInOut instance;
    public static SceneFadeInOut Instance => instance;

    private Image image;

    void Awake()
    {
        instance = this;
        //得到自身Image组件
        image = this.GetComponent<Image>();
    }

    //场景渐入，遮挡图片变黑
    public void FadeIn(float duration)
    {
        image.DOBlendableColor(Color.black, duration);
    }

    //场景渐出，遮挡图片变透明
    public void FadeOut(float duration)
    {
        image.DOBlendableColor(Color.clear, duration);
    }
}
