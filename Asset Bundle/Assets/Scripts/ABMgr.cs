using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ֪ʶ��
/// 1.AB����ص�Api
/// 2.����ģʽ
/// 3.ί��->Lambad���ʽ
/// 4.�ֵ�
/// 5.Э��
/// </summary>

public class ABMgr : MonoBehaviour
{
    private static ABMgr instance;
    public static ABMgr Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    //����
    private AssetBundle mainAB = null;
    //��������ȡ�õ������ļ�
    private AssetBundleManifest manifest = null;

    //AB�������ظ����� �ظ����ػᱨ��
    //�ֵ� ���ֵ�洢 ���ع���AB��
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    //AB���洢·���������޸�
    private string PathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }

    //������ �����޸�
    private string MainABName
    {
        get
        {
#if Unity_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android";
#else
            return "PC";
#endif
        }
    }

    public void LoadAB(string abName)
    {
        //��������
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //����������
        AssetBundle ab = null;
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }

        //����AB��
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }
    }

    //ͬ������ ��ָ������
    public Object LoadRes(string abName, string resName)
    {
        //����AB��
        LoadAB(abName);
        //������Դ
        Object obj = abDic[abName].LoadAsset(resName);
        if (obj is GameObject)
        {
            return Instantiate(obj);
        }
        else
            return obj;
    }

    //ͬ������ ����Typeָ������
    public Object LoadRes(string abName, string resName, System.Type type)
    {
        LoadAB(abName);
        //������Դ
        Object obj = abDic[abName].LoadAsset(resName, type);
        if (obj is GameObject)
        {
            return Instantiate(obj);
        }
        else
            return obj;
    }

    //ͬ������ ���ݷ���ָ������
    public T LoadRes<T>(string abName, string resName) where T : Object
    {
        LoadAB(abName);
        //������Դ
        T obj = abDic[abName].LoadAsset<T>(resName);
        if (obj is GameObject)
        {
            return Instantiate(obj);
        }
        else
            return obj;
    }

    //�첽����
    //��ָ������
    public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, callBack));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        //����AB��
        LoadAB(abName);
        //������Դ
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        yield return abr;
        if (abr.asset is GameObject)
        {
            callBack(Instantiate(abr.asset));
        }
        else
            callBack(abr.asset);
    }

    //����Typeָ������
    public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, type, callBack));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type , UnityAction<Object> callBack)
    {
        //����AB��
        LoadAB(abName);
        //������Դ
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
        yield return abr;
        if (abr.asset is GameObject)
        {
            callBack(Instantiate(abr.asset));
        }
        else
            callBack(abr.asset);
    }

    //���ݷ���ָ������
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callBack));
    }

    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T: Object
    {
        //����AB��
        LoadAB(abName);
        //������Դ
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abr;
        if (abr.asset is GameObject)
        {
            callBack(Instantiate(abr.asset) as T);
        }
        else
            callBack(abr.asset as T);
    }

    //������ж��
    public void UnLoad(string abName)
    {
        if(abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }

    //���а�ж��
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }
}
