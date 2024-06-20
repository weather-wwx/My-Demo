using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        Object obj = ABMgr.Instance.LoadRes("model", "Cube");
        Instantiate(obj);
    }
}
