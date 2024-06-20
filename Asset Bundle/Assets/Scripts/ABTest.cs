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
        //1.����AB�� AB�������ظ����أ����򱨴�
        AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "model");

        //�������Ĺؼ�֪ʶ-�������� ��ȡ������Ϣ
        //��������
        AssetBundle abMain = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "PC");
        //���������еĹ̶��ļ�
        AssetBundleManifest assetBundleManifest = abMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //�ӹ̶��ļ��� ��ȡ������Ϣ
        string[] strs = assetBundleManifest.GetAllDependencies("model");
        //�õ�������������
        for (int i = 0; i < strs.Length; i++)
        {
            AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + strs[i]);
        }

        //2.����AB���е���Դ
        GameObject gameObject = ab.LoadAsset<GameObject>("Cube");
        Instantiate(gameObject, Vector3.zero, Quaternion.identity);

       //StartCoroutine(LoadABRes("img","1"));

        //AB����ж��
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
