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
        //�õ�����Image���
        image = this.GetComponent<Image>();
    }

    //�������룬�ڵ�ͼƬ���
    public void FadeIn(float duration)
    {
        image.DOBlendableColor(Color.black, duration);
    }

    //�����������ڵ�ͼƬ��͸��
    public void FadeOut(float duration)
    {
        image.DOBlendableColor(Color.clear, duration);
    }
}
