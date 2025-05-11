using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePanelController : MonoBehaviour
{
    public RectTransform panel; // Панель, которую нужно скрывать/показывать
    public float offScreenX = -500f; // Координата X для скрытого состояния
    public float onScreenX = 0f; // Координата X для видимого состояния
    public float speed = 5f; // Скорость анимации

    private bool isVisible = true; // Состояние панели

    private void Start()
    {
        offScreenX = panel.sizeDelta.x * (-1);
        onScreenX = panel.position.x;
    }

    public void TogglePanel()
    {
        StopAllCoroutines(); // Остановить текущие корутины, если они есть
        StartCoroutine(SlidePanel(isVisible ? offScreenX : onScreenX));
        isVisible = !isVisible;
    }

    private System.Collections.IEnumerator SlidePanel(float targetX)
    {
        while (Mathf.Abs(panel.anchoredPosition.x - targetX) > 0.1f)
        {
            float newX = Mathf.Lerp(panel.anchoredPosition.x, targetX, Time.deltaTime * speed);
            panel.anchoredPosition = new Vector2(newX, panel.anchoredPosition.y);
            yield return null;
        }
        panel.anchoredPosition = new Vector2(targetX, panel.anchoredPosition.y);
    }
}
