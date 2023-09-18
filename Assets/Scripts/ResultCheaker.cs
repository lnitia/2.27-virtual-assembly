using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class ResultCheaker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textObject;
    [SerializeField] private GameObject sixAxleBogie;
    
    void Update()
    {
        bool interferenceDetected = false;

        foreach (Transform child in sixAxleBogie.transform)
        {
            if (child.gameObject.activeSelf && child.gameObject.name.Contains("Wheelset"))
            {
                Renderer wheelRenderer = child.GetComponent<Renderer>();
                if (wheelRenderer != null)
                {
                    Color wheelColor = wheelRenderer.material.color;
                    int wheelsetNumber = GetWheelsetNumber(child.gameObject.name);
                    
                    if (wheelColor == Color.white)
                    {
                        textObject.text = $"轮对{wheelsetNumber}未发生干涉";
                        interferenceDetected = true;
                    }
                    else if (wheelColor == Color.red)
                    {
                        textObject.text = $"轮对{wheelsetNumber}发生干涉";
                        interferenceDetected = true;
                    }
                }
            }
        }

        if (!interferenceDetected)
        {
            textObject.text = "无";
        }
    }
    
    int GetWheelsetNumber(string wheelsetName)
    {
        // Extract the number from the wheelset name using regular expression
        Match match = Regex.Match(wheelsetName, @"\d+");
        if (match.Success)
        {
            return int.Parse(match.Value);
        }
        return -1; // Return -1 if no valid number is found
    }
}
