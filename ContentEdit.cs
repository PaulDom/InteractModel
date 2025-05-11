using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class ContentEdit : MonoBehaviour
{
    public GameObject targetObjectPrefab;
    public GameObject contentUI;

    public void addNewObject(GameObject objectToAdd)
    {
        if (IsDuplicate(objectToAdd))
        {
            return;
        }
        
        GameObject newObject = Instantiate(targetObjectPrefab, contentUI.transform);
        TargetUI script = newObject.GetComponent<TargetUI>();
        if (script != null)
        {
            script.Initialize(objectToAdd);
        }
    }

    private bool IsDuplicate(GameObject objectToCheck)
    {
        foreach (Transform child in contentUI.transform)
        {
            TargetUI targetScript = child.GetComponent<TargetUI>();
            if (targetScript != null && targetScript.target == objectToCheck)
            {
                return true;
            }
        }
        return false;
    }
}
