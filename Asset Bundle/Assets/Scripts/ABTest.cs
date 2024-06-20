using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ABTest : MonoBehaviour
{
    public Image img;

    void Start()
    {
        //1.加载AB包 AB包不能重复加载，否则报错
        AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "model");

        //依赖包的关键知识-利用主包 获取依赖信息
        //加载主包
        AssetBundle abMain = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "PC");
        //加载主包中的固定文件
        AssetBundleManifest assetBundleManifest = abMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //从固定文件中 获取依赖信息
        string[] strs = assetBundleManifest.GetAllDependencies("model");
        //得到依赖包的名字
        for (int i = 0; i < strs.Length; i++)
        {
            AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + strs[i]);
        }

        //2.加载AB包中的资源
        GameObject gameObject = ab.LoadAsset<GameObject>("Cube");
        Instantiate(gameObject, Vector3.zero, Quaternion.identity);

       //StartCoroutine(LoadABRes("img","1"));

        //AB包的卸载
        //ab.Unload(false);
        //Resources.UnloadUnusedAssets();
    }

    IEnumerator LoadABRes(string ABName, string resName)
    {
        AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + ABName);
        yield return abcr;
        AssetBundleRequest abr = abcr.assetBundle.LoadAssetAsync(resName, typeof(Sprite));
        yield return abr;
        img.sprite = abr.asset as Sprite;
    }
}
