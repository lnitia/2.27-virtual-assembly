using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChange : MonoBehaviour
{
    public void ChangeScale(float scaleFactor)
    {
        transform.localScale = new Vector3(scaleFactor,scaleFactor,scaleFactor);
    }
}
