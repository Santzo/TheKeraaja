using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuitPanelButton : MonoBehaviour, IUIObject
{
    Image image;
    Color oriColor;
    private void Awake()
    {
        image = GetComponent<Image>();
        oriColor = image.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        image.color = Settings.buttonPressed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.color = oriColor;
        if (name=="Kylla")
        {
            Application.Quit();
        }
        else
        {
            transform.parent.parent.gameObject.SetActive(false);
        }
    }


}
