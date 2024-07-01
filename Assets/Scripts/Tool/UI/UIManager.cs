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

    //���� �洢����UI�����ֵ� 
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private RectTransform canvas;


    public UIManager()
    {
        //����Canvas
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvas = obj.transform as RectTransform;
        //�л���Ϸ����ʱ��������canvas
        GameObject.DontDestroyOnLoad(obj);

        //����EventSystem ������ʱ����������
        obj = GameObject.Instantiate( Resources.Load<GameObject>("UI/EventSystem"));
        GameObject.DontDestroyOnLoad(obj);
    }

    //��ʾ���
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
        //ʵ���� ������
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + name));
        //�������ĸ����� ΪCanvas
        panelObj.transform.SetParent(canvas, false);
        //������ű�
        T panel = panelObj.GetComponent<T>();

        //�����洢���ֵ���  ��������ȡ
        panelDic.Add(name, panel);

        panel.ShowMe(isFade);

        if (callBack != null)
        {
            callBack(panel);
        }
    }

    //�������
    public void HidePanel<T>(string name, bool isFade) where T : BasePanel
    {
        if (panelDic.ContainsKey(name))
        {
            //���뽥��
            if (isFade)
            {
                panelDic[name].HideMe(() =>
                {
                    GameObject.Destroy(panelDic[name].gameObject);
                    panelDic.Remove(name);
                });
            }
            //ֱ��ɾ��
            else
            {
                GameObject.Destroy(panelDic[name].gameObject);
                panelDic.Remove(name);
            }
        }
    }

    //������
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if (panelDic.ContainsKey(name))
        {
            return panelDic[name] as T;
        }
        return null;
    }
}
