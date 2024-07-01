using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BasePanel : MonoBehaviour
{
    private static BasePanel instance;
    public static BasePanel Instance => instance;

    //ͨ������ת��ԭ�� ���洢���еĿؼ�
    private Dictionary<string, List<UIBehaviour>> controlDic = new
        Dictionary<string, List<UIBehaviour>>();

    //������Ƶ��뵭���� �������
    private CanvasGroup canvasGroup;
    //��嵭�뵭�����ٶ�
    private float alphaSpeed = 1;
    //�Ƿ�ʼ��ʾ
    public bool isShow = false;

    private UnityAction hidecallBack;

    protected virtual void Awake()
    {
        instance = this;

        //�õ��������
        canvasGroup = this.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();

        //��Ѱ������
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<Slider>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<InputField>();
    }

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Update()
    {
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
            }
        }
        else if (!isShow)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //ִ��ɾ��
                hidecallBack?.Invoke();
            }
        }
    }

    //��ʼ�� ���
    public abstract void Init();

    protected virtual void OnClick(string btnName)
    {

    }

    protected virtual void OnValueChanged(string toggleName, bool value)
    {

    }

    //�õ���Ӧ���ֵĶ�Ӧ�ؼ��ű�
    public T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            for (int i = 0; i < controlDic[controlName].Count; i++)
            {
                if (controlDic[controlName][i] is T)
                    return controlDic[controlName][i] as T;
            }
        }
        return null;
    }

    /// <summary>
    /// �ҵ��Ӷ������϶�Ӧ�Ŀؼ�        
    /// </summary>
    /// <typeparam name="T">�ؼ�����</typeparam>
    public void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>();
        for (int i = 0; i < controls.Length; i++)
        {
            string objName = controls[i].gameObject.name;
            if (controlDic.ContainsKey(objName))
            {
                controlDic[objName].Add(controls[i]);
            }
            else
            {
                controlDic.Add(objName, new List<UIBehaviour>() {
                    controls[i]
                });
            }

            //����ǰ�ť�ؼ�
            if (controls[i] is Button)
            {
                (controls[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }
            //����ǵ�ѡ����߶�ѡ��
            else if (controls[i] is Toggle)
            {
                (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged(objName, value);
                });
            }

        }
    }

    //��ʾ�����
    public virtual void ShowMe(bool isFade)
    {
        if(isFade)
        {
            isShow = true;
            canvasGroup.alpha = 0;
        }
        else
        {
            isShow = true;
            canvasGroup.alpha = 1;
        }
    }

    //���ظ����
    public virtual void HideMe(UnityAction callback)
    {
        isShow = false;
        canvasGroup.alpha = 1;
        hidecallBack = callback;
    }
}
