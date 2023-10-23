using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFollower : MonoBehaviour
{
    [SerializeField] private Transform main;
    [SerializeField] private Transform[] followers;
    private Vector3 lastMainScale;
    
    void Start()
    {
        lastMainScale = main.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (main.localScale != lastMainScale)
        {
            foreach (var fol in followers)
            {
                fol.localScale = main.localScale;
            }

            lastMainScale = main.localScale;
        }
    }
}
