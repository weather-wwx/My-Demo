using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum JsonType
{
    JsonUtility,
    LitJson
}

public class JsonMgr 
{
    private static JsonMgr instance=new JsonMgr();
    public static JsonMgr Instance => instance;
    
    private JsonMgr() { }

    //�����ݴ��ڴ��д洢��Ӳ���� ���л�
    public void SaveData(object data,string fileName,JsonType type=JsonType.LitJson)
    {
        //���л� �õ�Json�ַ���
        string jsonStr="";
        if (type==JsonType.LitJson)
        {
            jsonStr = JsonMapper.ToJson(data);
        }
        else
        {
            jsonStr = JsonUtility.ToJson(data);

        }
        File.WriteAllText(Application.persistentDataPath + "/" + fileName + ".json", jsonStr);
    }

    //�����ݴ�Ӳ���ж�ȡ���ڴ� �����л�
    public T LoadData<T>(string fileName,JsonType type=JsonType.LitJson) where T : new()
    {
        //���ж�Ĭ���ļ� �Ƿ���������Ҫ������ ����� �ʹ��л�ȡ
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        if(!File.Exists(path))
        {
            path = Application.persistentDataPath + "/" + fileName + ".json";
        }
        //�����д�ļ���û�� ����Ĭ��ֵ    
        if (!File.Exists(path))
            return new T();

        string jsonStr=File.ReadAllText(path);
        //�����л�
        if (type==JsonType.LitJson)
        {
            return JsonMapper.ToObject<T>(jsonStr);
        }
        else
        {
            return JsonUtility.FromJson<T>(jsonStr);
        }
    }
}
