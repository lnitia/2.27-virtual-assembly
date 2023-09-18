using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterChange : MonoBehaviour
{
    public GameObject changeToGameObject;
    // Start is called before the first frame update
    void Start()
    {
        OnChangeCenter();
        //Debug.Log("对象网格中心在世界坐标系位置：" + gameObject.GetComponent<MeshRenderer>().bounds.center.ToString("f4")); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnChangeCenter()
    {
        Debug.Log("2:" + gameObject.GetComponent<MeshRenderer>().bounds.center.ToString("f4")); 
        //gameObject.GetComponent<Transform>().localPosition = changeToGameObject.GetComponent<MeshRenderer>().bounds.center;
        Debug.Log("1：" + changeToGameObject.GetComponent<MeshRenderer>().bounds.center.ToString("f4") + "- 2：" + gameObject.GetComponent<MeshRenderer>().bounds.center.ToString("f4"));
    }
}
