using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using SixAxisBogieAssembly;
using UnityEngine;

public class OriginWheelsetHandler : MonoBehaviour
{
    [SerializeField] private GameObject originWheelset;
    [SerializeField] private List<Interactable> buttons;
    [SerializeField] private List<AssemblyController> replaceWheelsets;

    private MeshRenderer mr;
    private void Start()
    {
        mr = originWheelset.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        foreach (var button in buttons)
        {
            if (button.IsToggled)
            {
                mr.material = Resources.Load<Material>("XRAY");
                break;
            }
            
            mr.material = Resources.Load<Material>("MRTKMaterial");
        }
    }

    public void ResetMC(int i)
    {
        AssemblyController rep = replaceWheelsets[i - 1];
        if (!rep.isPlaced)
        {
            foreach (var tri in rep.otherColliders)
                tri.enabled = false;
        }
    }
}
