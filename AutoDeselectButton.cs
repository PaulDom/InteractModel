using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoDeselectButton : MonoBehaviour
{
    private void Update()
    {
        // Если кнопка выделена и игрок нажимает кнопку мыши (или тач)
        if (EventSystem.current.currentSelectedGameObject != null && Input.GetMouseButtonDown(0))
        {
            // Сбросить выделение
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
