using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleMgr
{
    private static MultipleMgr instance = new MultipleMgr();
    public static MultipleMgr Instatnce => instance;

    //�洢 ��ͼ��Ӧ��Сͼ��Դ����Ϣ
    private Dictionary<string, Dictionary<string, Sprite>> dic = new Dictionary<string, Dictionary<string, Sprite>>();
    private MultipleMgr()
    {

    }

    /// <summary>
    /// ��ȡMultipleͼ���е�ĳһ��Сͼ
    /// </summary>
    /// <param name="multipleName">ͼ����</param>
    /// <param name="spriteName">����ͼƬ��</param>
    /// <returns></returns>
    public Sprite GetSprite(string multipleName, string spriteName)
    {
        //�ж��Ƿ���ع��ô�ͼ
        if (dic.ContainsKey(multipleName))
        {
            //�жϴ�ͼ���Ƿ��и�Сͼ����Ϣ
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
            //�ж� �Ƿ��и����ֵ�Сͼ
            if (dicTmp.ContainsKey(spriteName))
                return dicTmp[spriteName];
        }

        return null;
    }

    public void ClearInfo()
    {
        //���
        dic.Clear();
        //ж����Դ
        Resources.UnloadUnusedAssets();
    }
}
