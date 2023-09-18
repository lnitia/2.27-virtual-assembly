using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshStandard : MonoBehaviour
{
    //��ȡ����
    private string _node;
    private string _tab;
    private string _p;
    //��������
    private List<float> numberList1 = new List<float>();
    private List<int> numberList2 = new List<int>();
    private List<float> numberList3 = new List<float>();

    string _nodeData_Space;
    string[] _nodeData_Array;
    float[] _nodeData_FloatArray;

    string _tabData_Space;
    string[] _tabData_Array;
    int[] _tabData_IntArray;

    float _pData;
    //���泤��
    string _nodeLength;
    int verticeLength;
    string _tabLength;
    int triangleLength;
    string _pLength;
    int colorLength;
    //��Ԫֵ��Χ
    int _pmax = 500;
    int _pmin = -500;

    void Start()
    {
        ReadData();
        CreateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ReadData()
    {
        //��ȡ�ڵ�����
        XmlDocument xmlnode = new XmlDocument();
        //xml.Load(Application.streamingAssetsPath + "/Slun_node.xml");
        TextAsset assetnode = Resources.Load<TextAsset>("Slun_node");
        print(assetnode.text);
        // ͨ��������� ���ܹ������ַ���Ϊxml����
        xmlnode.LoadXml(assetnode.text);
        //  ��ȡXML�еĸ��ڵ�
        XmlNode nodes = xmlnode.SelectSingleNode("Nodes");
        //  ��ͨ�����ڵ�ȥ��ȡ������ӽڵ�
        //XmlNode node = nodes.SelectSingleNode("node");
        //  ��ȡ�ڵ��а���������
        // print(node.Attributes["id"].Value);
        XmlNodeList nodeList = nodes.SelectNodes("node");
        //  ͨ���������List�Ϳ��Եõ����е������ӽڵ���
        foreach (XmlNode item in nodeList)
        {
            //print(item.Attributes["id"].Value);
            //print(item.SelectSingleNode("gcoord").InnerText);
            _node = item.SelectSingleNode("gcoord").InnerText;
            //SPlit����ֻ�ָܷ���ո����������ո��滻��һ���ո�
            _nodeData_Space = _node.Replace("  ", " ");
            //�ָ��ַ���
            _nodeData_Array = _nodeData_Space.Split(' ');
            //�����ȡ��������
            _nodeData_FloatArray = new float[3];
            for (int i = 0; i < 3; i++)
            {
                _nodeData_FloatArray[i] = float.Parse(_nodeData_Array[i]);
            }
            //��ӵ�List������
            for (int i = 0; i < 3; i++)
            {
                numberList1.Add(_nodeData_FloatArray[i]);
            }
            _nodeLength = item.Attributes["id"].Value;
        }
        verticeLength = int.Parse(_nodeLength);
        //print(verticeLength);

        //��ȡ�ڵ�����
        XmlDocument xmltab = new XmlDocument();
        TextAsset assettab = Resources.Load<TextAsset>("Slun_tab");

        print(assettab.text);
        xmltab.LoadXml(assettab.text);

        XmlNode Element = xmltab.SelectSingleNode("Elements");

        XmlNodeList ElementList = Element.SelectNodes("Element");

        foreach (XmlNode item in ElementList)
        {
            //print(item.Attributes["id"].Value);
            //print(item.SelectSingleNode("node").InnerText);

            _tab = item.SelectSingleNode("node").InnerText;
            _tabData_Space = _tab.Replace("  ", " ");
            _tabData_Array = _tabData_Space.Split(' ');
            _tabData_IntArray = new int[3];

            for (int i = 0; i < 3; i++)
            {
                _tabData_IntArray[i] = int.Parse(_tabData_Array[i]);
            }
            for (int i = 0; i < 3; i++)
            {
                numberList2.Add(_tabData_IntArray[i]);
            }
            _tabLength = item.Attributes["id"].Value;
        }
        triangleLength = int.Parse(_tabLength) * 3;
        //print(triangleLength);


        //��ȡ��Ԫֵ
        XmlDocument xmlp = new XmlDocument();
        TextAsset assetp = Resources.Load<TextAsset>("Slun_p");

        print(assetp.text);
        xmlp.LoadXml(assetp.text);

        XmlNode Elementp = xmlp.SelectSingleNode("Elements");

        XmlNodeList ElementListp = Elementp.SelectNodes("Element");

        foreach (XmlNode item in ElementListp)
        {
            //print(item.Attributes["id"].Value);
            //print(item.SelectSingleNode("Solid_P").InnerText);
            _p = item.SelectSingleNode("Solid_P").InnerText;
            _pData = float.Parse(_p);
            numberList3.Add(_pData);
            _pLength = item.Attributes["id"].Value;
        }
        colorLength = int.Parse(_pLength);
        //print(colorLength);
    }


    void CreateMesh()
    {

        //GameObject obj = new GameObject("mesh");
        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();


        Vector3[] vertices = new Vector3[verticeLength];//[verticeLength];
        int[] triangles = new int[triangleLength];//[triangleLength];
        Color[] colors = new Color[colorLength];//[colorLength];

        //�ڵ����긳����������
        for (int i = 0, vi = 0; i < vertices.Length; i++, vi += 3)
        {
            vertices[i].Set(numberList1[vi], numberList1[vi + 1], numberList1[vi + 2]);
            //print(vertices[i]);
        }

        //��Ԫ���Ӹ�������������
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = numberList2[i] - 1;
            //print(triangles[i]);
        }

        //��Ԫֵת���ɶ�����ɫ
        int _range = _pmax - _pmin + 1;
        //��_dataӳ�䵽��ͬ��ɫ��������
        for (int i = 0; i < colors.Length; i++)
        {
            float _data = numberList3[i];
            float r = (_data - _pmin) / _range;
            int step = _range / 5;
            int idx = (int)(r * 5.0);
            int h = (idx + 1) * step + _pmin;
            int m = idx * step + _pmin;
            float local_r = (_data - m) / (h - m);
            if (_data < _pmin)
                colors[i] = new Color(0, 0, 0);
            if (_data > _pmax)
                colors[i] = new Color(1, 1, 1);
            if (idx == 0)
                colors[i] = new Color(0, local_r, 1);
            if (idx == 1)
                colors[i] = new Color(0, 1, 1 - local_r);
            if (idx == 2)
                colors[i] = new Color(local_r, 1, 0);
            if (idx == 3)
                colors[i] = new Color(1, 1 - local_r, 0);
            if (idx == 4)
                colors[i] = new Color(1, 0, local_r);
        }

        mf.mesh.vertices = vertices;
        mf.mesh.triangles = triangles;
        mf.mesh.colors = colors;
        mf.mesh.RecalculateNormals();
        mf.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mr.material = new Material(Shader.Find("Mixed Reality Toolkit/Standard"));

    }
}

