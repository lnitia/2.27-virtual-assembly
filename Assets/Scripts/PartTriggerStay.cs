using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Threading;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;

public class PartTriggerStay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        //collider.GetComponent<MeshOutline>().enabled = true;
        Debug.Log(collider + "ENTER" + gameObject);
    }

    private void OnTriggerStay(Collider collider)
    {
        MeshRenderer mr = collider.GetComponent<MeshRenderer>();
        for (int i = 0; i < mr.materials.Length; i++) 
        {
            mr.materials[i].color = Color.red;
        } 
        //Debug.Log("STAY" + collider);
    }
  
    private void OnTriggerExit(Collider collider)
    {
        MeshRenderer mr = collider.GetComponent<MeshRenderer>();
        for (int i = 0; i < mr.materials.Length; i++)
        {
            mr.materials[i].color = Color.white;
        }
        //collider.GetComponent<MeshRenderer>().material.color = Color.white;
        //collider.GetComponent<MeshOutline>().enabled = false;
        
        Debug.Log(collider + "EXIT" + gameObject);
    }

}
