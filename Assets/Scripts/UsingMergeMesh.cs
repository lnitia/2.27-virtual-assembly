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

        MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();//��ȡ���������������


        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length]; //�½�һ���ϲ��飬������ meshfiltersһ��
        for (int i = 0; i < meshFilters.Length; i++)//����
        {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;//������mesh����ֵ
            combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix; //��������ת���󣬸�ֵ
            GameObject.DestroyImmediate(meshFilters[i].gameObject);
        }
        Mesh newMesh = new Mesh();//����һ�����������
        newMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        newMesh.CombineMeshes(combineInstances, mergeSubMeshes, true);//��combineInstances���鴫�뺯��
        parent.AddComponent<MeshFilter>().sharedMesh = newMesh; //����ǰ�����壬���������������ϲ�������񣬸�����������
        //parent.GetComponent<MeshCollider>.mesh =  

        parent.AddComponent<MeshRenderer>().sharedMaterials = materials;

        //Vector3 offsetP = parent.GetComponent<UsingMergeMesh>().offsetP;
        //Vector3 offsetR = parent.GetComponent<UsingMergeMesh>().offsetR;
        //parent.GetComponent<Transform>().localPosition = new Vector3(offsetP.x, offsetP.y,offsetP.z);
        //parent.GetComponent<Transform>().localEulerAngles = new Vector3(offsetR.x, offsetR.y, offsetR.z);
    }
}