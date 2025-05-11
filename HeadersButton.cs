using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadersButton : MonoBehaviour
{
    public GameObject contentUI;
    private bool _isSelectedCheckBox = false;
    private bool _isSelectedViewObject = true;
    
    public void SelectedAllCheckBoxButton()
    {
        _isSelectedCheckBox = !_isSelectedCheckBox;
        
        foreach (Transform child in contentUI.transform)
        {
            Toggle toggle = child.Find("Toggle")?.GetComponent<Toggle>();
            toggle.isOn = _isSelectedCheckBox;
        }
    }

    public void ViewChangeButton()
    {
        _isSelectedViewObject = !_isSelectedViewObject;
        
        foreach (Transform child in contentUI.transform)
        {
            Toggle toggle = child.Find("Toggle")?.GetComponent<Toggle>();
            if (toggle.isOn)
            {
                TargetUI script = toggle.GetComponentInParent<TargetUI>();
                script.target.SetActive(_isSelectedViewObject);
            }
        }
    }

    public void SetTransparencyButton(float alpha)
    {
        alpha = Mathf.Clamp01(alpha);
        
        foreach (Transform child in contentUI.transform)
        {
            Toggle toggle = child.Find("Toggle")?.GetComponent<Toggle>();
            if (toggle.isOn)
            {
                TargetUI script = toggle.GetComponentInParent<TargetUI>();
                Renderer renderer = script.target.GetComponent<Renderer>();
                foreach (var mat in renderer.materials)
                {
                    Color color = mat.color;
                    color.a = alpha;
                    mat.color = color;
                }
            }
        }
    }
}
