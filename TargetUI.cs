using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TargetUI : MonoBehaviour
{
    public GameObject target;
    private TextMeshProUGUI textComponent;

    public void ChangeLabel()
    {
        if (textComponent != null)
        {
            textComponent.text = target.name;
        }
        else
        {
            textComponent.text = "No target found";
        }
    }

    public void Initialize(GameObject target)
    {
        this.target = target;
        textComponent = transform.Find("Toggle/Label")?.GetComponent<TextMeshProUGUI>();
        ChangeLabel();
    }
}
