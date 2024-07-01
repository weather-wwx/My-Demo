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

    //通过里氏转换原则 来存储所有的控件
    private Dictionary<string, List<UIBehaviour>> controlDic = new
        Dictionary<string, List<UIBehaviour>>();

    //整体控制淡入淡出的 画布组件
    private CanvasGroup canvasGroup;
    //面板淡入淡出的速度
    private float alphaSpeed = 1;
    //是否开始显示
    public bool isShow = false;

    private UnityAction hidecallBack;

    protected virtual void Awake()
    {
        instance = this;

        //得到画布组件
        canvasGroup = this.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();

        //找寻相关组件
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
                //执行删除
                hidecallBack?.Invoke();
            }
        }
    }

    //初始化 面板
    public abstract void Init();

    protected virtual void OnClick(string btnName)
    {

    }

    protected virtual void OnValueChanged(string toggleName, bool value)
    {

    }

    //得到对应名字的对应控件脚本
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
    /// 找到子对象身上对应的控件        
    /// </summary>
    /// <typeparam name="T">控件类型</typeparam>
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

            //如果是按钮控件
            if (controls[i] is Button)
            {
                (controls[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }
            //如果是单选框或者多选框
            else if (controls[i] is Toggle)
            {
                (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged(objName, value);
                });
            }

        }
    }

    //显示该面板
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

    //隐藏该面板
    public virtual void HideMe(UnityAction callback)
    {
        isShow = false;
        canvasGroup.alpha = 1;
        hidecallBack = callback;
    }
}
