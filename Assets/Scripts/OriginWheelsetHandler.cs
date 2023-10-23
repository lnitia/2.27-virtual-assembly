using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using SixAxisBogieAssembly;
using UnityEngine;

public class OriginWheelsetHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] originWheelsets;
    [SerializeField] private List<Interactable> buttons;
    [SerializeField] private List<AssemblyController> replaceWheelsets;

    private MeshRenderer[] mr;
    
    private void Start()
    {
        mr = new MeshRenderer[originWheelsets.Length];

        for(int i = 0; i < originWheelsets.Length; i++)
        {
            mr[i] = originWheelsets[i].GetComponent<MeshRenderer>();
        }
    }

    private void Update()
    {
        bool anyButtonToggled = false;

        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].IsToggled)
            {
                anyButtonToggled = true;
                break;
            }
        }

        for (int i = 0; i < mr.Length; i++)
        {
            if (anyButtonToggled)
            {
                mr[i].material = Resources.Load<Material>("XRAY");
            }
            else
            {
                mr[i].material = Resources.Load<Material>("MRTKMaterial");
            }
        }
    }

    public void ResetMC(int num)
    {
        for (int i = 0; i < replaceWheelsets.Count; i++)
        {
            foreach (var tri in replaceWheelsets[i].otherColliders)
                tri.enabled = false;
        }
        
        AssemblyController rep = replaceWheelsets[num - 1];
        if (!rep.isPlaced)
        {
            foreach (var tri in rep.otherColliders)
                tri.enabled = false;
        }
        else if (rep.isPlaced)
        {
            foreach (var tri in rep.otherColliders)
                tri.enabled = true;
        }
    }
}
