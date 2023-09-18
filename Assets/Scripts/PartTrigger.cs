using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PartTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        collider.GetComponent<MeshRenderer>().material.color =new Color(0,0,1,0.8f);
        //Color a = collider.GetComponent<MeshRenderer>().material.color;
        //collider.GetComponent<MeshRenderer>().material.color = new Color(a.r, a.g, a.b, 0.8f);
        Debug.Log("ENTER");
    }

    private void OnTriggerStay(Collider collider)
    {
        //Debug.Log("STAY");
    }

    private void OnTriggerExit(Collider collider)
    {
        collider.GetComponent<MeshRenderer>().material.color = Color.white;
        //Color a = collider.GetComponent<MeshRenderer>().material.color;
        //collider.GetComponent<MeshRenderer>().material.color = new Color(a.r, a.g, a.b,1.0f);
        Debug.Log("EXIT");
    }
}
