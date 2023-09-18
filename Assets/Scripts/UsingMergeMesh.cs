using Microsoft.MixedReality.Toolkit.Experimental.StateVisualizer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UsingMergeMesh : MonoBehaviour
{
    //[SerializeField] private Vector3 offsetP;
    //[SerializeField] private Vector3 offsetR;   
    // Start is called before the first frame update
    void Start()
    {
        MergeMesh(gameObject, false);
    }

    public static void MergeMesh(GameObject parent, bool mergeSubMeshes = false)
    {
        MeshRenderer[] meshRenderers = parent.GetComponentsInChildren<MeshRenderer>();
        Material[] materials = new Material[meshRenderers.Length];
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            materials[i] = meshRenderers[i].sharedMaterial;
        }

        MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();//获取所有子物体的网格


        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length]; //新建一个合并组，长度与 meshfilters一致
        for (int i = 0; i < meshFilters.Length; i++)//遍历
        {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;//将共享mesh，赋值
            combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix; //本地坐标转矩阵，赋值
            GameObject.DestroyImmediate(meshFilters[i].gameObject);
        }
        Mesh newMesh = new Mesh();//声明一个新网格对象
        newMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        newMesh.CombineMeshes(combineInstances, mergeSubMeshes, true);//将combineInstances数组传入函数
        parent.AddComponent<MeshFilter>().sharedMesh = newMesh; //给当前空物体，添加网格组件；将合并后的网格，给到自身网格
        //parent.GetComponent<MeshCollider>.mesh =  

        parent.AddComponent<MeshRenderer>().sharedMaterials = materials;

        //Vector3 offsetP = parent.GetComponent<UsingMergeMesh>().offsetP;
        //Vector3 offsetR = parent.GetComponent<UsingMergeMesh>().offsetR;
        //parent.GetComponent<Transform>().localPosition = new Vector3(offsetP.x, offsetP.y,offsetP.z);
        //parent.GetComponent<Transform>().localEulerAngles = new Vector3(offsetR.x, offsetR.y, offsetR.z);
    }
}