using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleMgr
{
    private static MultipleMgr instance = new MultipleMgr();
    public static MultipleMgr Instatnce => instance;

    //存储 大图对应的小图资源的信息
    private Dictionary<string, Dictionary<string, Sprite>> dic = new Dictionary<string, Dictionary<string, Sprite>>();
    private MultipleMgr()
    {

    }

    /// <summary>
    /// 获取Multiple图集中的某一张小图
    /// </summary>
    /// <param name="multipleName">图集名</param>
    /// <param name="spriteName">单张图片名</param>
    /// <returns></returns>
    public Sprite GetSprite(string multipleName, string spriteName)
    {
        //判断是否加载过该大图
        if (dic.ContainsKey(multipleName))
        {
            //判断大图中是否有该小图的信息
            if (dic[multipleName].ContainsKey(spriteName))
                return dic[multipleName][spriteName];
        }
        else
        {
            Dictionary<string, Sprite> dicTmp = new Dictionary<string, Sprite>();
            Sprite[] sprs = Resources.LoadAll<Sprite>("Icon/" + multipleName);
            for (int i = 0; i < sprs.Length; i++)
            {
                dicTmp.Add(sprs[i].name, sprs[i]);
            }

            dic.Add(multipleName, dicTmp);
            //判断 是否有该名字的小图
            if (dicTmp.ContainsKey(spriteName))
                return dicTmp[spriteName];
        }

        return null;
    }

    public void ClearInfo()
    {
        //清空
        dic.Clear();
        //卸载资源
        Resources.UnloadUnusedAssets();
    }
}
