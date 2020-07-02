using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    IMainMenuHandler menuHandler;
    Image image;
    internal Color oriColor;
    public bool colorStaysWhenSelected;

    void Awake()
    {
        menuHandler = GetComponentInParent<IMainMenuHandler>();
        if (menuHandler == null)
        {
            Debug.Log("No Main Menu handler found for " + name);
        }
        image = GetComponentInParent<Image>();
        image.enabled = true;
        oriColor = image.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        menuHandler.OnDown(transform);
        image.color = Settings.buttonPressed;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        menuHandler.OnUp(transform);
        if (!colorStaysWhenSelected) image.color = oriColor;
    }
}
