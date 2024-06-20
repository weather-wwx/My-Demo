using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 知识点
/// 1.AB包相关的Api
/// 2.单例模式
/// 3.委托->Lambad表达式
/// 4.字典
/// 5.协程
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

    //主包
    private AssetBundle mainAB = null;
    //依赖包获取用的配置文件
    private AssetBundleManifest manifest = null;

    //AB包不能重复加载 重复加载会报错
    //字典 用字典存储 加载过的AB包
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    //AB包存储路径，方便修改
    private string PathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }

    //主包名 方便修改
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
        //加载主包
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //加载依赖包
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

        //加载AB包
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }
    }

    //同步加载 不指定类型
    public Object LoadRes(string abName, string resName)
    {
        //加载AB包
        LoadAB(abName);
        //加载资源
        Object obj = abDic[abName].LoadAsset(resName);
        if (obj is GameObject)
        {
            return Instantiate(obj);
        }
        else
            return obj;
    }

    //同步加载 根据Type指定类型
    public Object LoadRes(string abName, string resName, System.Type type)
    {
        LoadAB(abName);
        //加载资源
        Object obj = abDic[abName].LoadAsset(resName, type);
        if (obj is GameObject)
        {
            return Instantiate(obj);
        }
        else
            return obj;
    }

    //同步加载 根据泛型指定类型
    public T LoadRes<T>(string abName, string resName) where T : Object
    {
        LoadAB(abName);
        //加载资源
        T obj = abDic[abName].LoadAsset<T>(resName);
        if (obj is GameObject)
        {
            return Instantiate(obj);
        }
        else
            return obj;
    }

    //异步加载
    //不指定类型
    public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, callBack));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        //加载AB包
        LoadAB(abName);
        //加载资源
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        yield return abr;
        if (abr.asset is GameObject)
        {
            callBack(Instantiate(abr.asset));
        }
        else
            callBack(abr.asset);
    }

    //根据Type指定类型
    public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, type, callBack));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type , UnityAction<Object> callBack)
    {
        //加载AB包
        LoadAB(abName);
        //加载资源
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
        yield return abr;
        if (abr.asset is GameObject)
        {
            callBack(Instantiate(abr.asset));
        }
        else
            callBack(abr.asset);
    }

    //根据泛型指定类型
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callBack));
    }

    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T: Object
    {
        //加载AB包
        LoadAB(abName);
        //加载资源
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abr;
        if (abr.asset is GameObject)
        {
            callBack(Instantiate(abr.asset) as T);
        }
        else
            callBack(abr.asset as T);
    }

    //单个包卸载
    public void UnLoad(string abName)
    {
        if(abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }

    //所有包卸载
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }
}
