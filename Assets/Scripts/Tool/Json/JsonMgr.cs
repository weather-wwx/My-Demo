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

    //将数据从内存中存储到硬盘中 序列化
    public void SaveData(object data,string fileName,JsonType type=JsonType.LitJson)
    {
        //序列化 得到Json字符串
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

    //将数据从硬盘中读取到内存 反序列化
    public T LoadData<T>(string fileName,JsonType type=JsonType.LitJson) where T : new()
    {
        //先判断默认文件 是否有我们需要的数据 如果有 就从中获取
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        if(!File.Exists(path))
        {
            path = Application.persistentDataPath + "/" + fileName + ".json";
        }
        //如果读写文件中没有 返回默认值    
        if (!File.Exists(path))
            return new T();

        string jsonStr=File.ReadAllText(path);
        //反序列化
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
