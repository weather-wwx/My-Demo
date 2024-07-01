using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }

    //创建 存储所有UI面板的字典 
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private RectTransform canvas;


    public UIManager()
    {
        //创建Canvas
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvas = obj.transform as RectTransform;
        //切换游戏场景时，不销毁canvas
        GameObject.DontDestroyOnLoad(obj);

        //创建EventSystem 过场景时不进行销毁
        obj = GameObject.Instantiate( Resources.Load<GameObject>("UI/EventSystem"));
        GameObject.DontDestroyOnLoad(obj);
    }

    //显示面板
    public void ShowPanel<T>(string name, bool isFade , UnityAction<T> callBack = null) where T : BasePanel
    {
        if (panelDic.ContainsKey(name))
        {
            panelDic[name].ShowMe(isFade);
            if (callBack != null)
            {
                callBack(panelDic[name] as T);
            }
            return;
        }
        //实例化 面板对象
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + name));
        //设置面板的父对象 为Canvas
        panelObj.transform.SetParent(canvas, false);
        //获得面板脚本
        T panel = panelObj.GetComponent<T>();

        //将面板存储到字典中  方便后面获取
        panelDic.Add(name, panel);

        panel.ShowMe(isFade);

        if (callBack != null)
        {
            callBack(panel);
        }
    }

    //隐藏面板
    public void HidePanel<T>(string name, bool isFade) where T : BasePanel
    {
        if (panelDic.ContainsKey(name))
        {
            //渐入渐出
            if (isFade)
            {
                panelDic[name].HideMe(() =>
                {
                    GameObject.Destroy(panelDic[name].gameObject);
                    panelDic.Remove(name);
                });
            }
            //直接删除
            else
            {
                GameObject.Destroy(panelDic[name].gameObject);
                panelDic.Remove(name);
            }
        }
    }

    //获得面板
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if (panelDic.ContainsKey(name))
        {
            return panelDic[name] as T;
        }
        return null;
    }
}
