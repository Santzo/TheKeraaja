using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    IMainMenuHandler menuHandler;
    Animator animator;
    Image image;

    void Awake()
    {
        menuHandler = GetComponentInParent<IMainMenuHandler>();
        if (menuHandler==null)
        {
            Debug.Log("No Main Menu handler found for " + name);
        }
        animator = GetComponentInParent<Animator>();
        image = GetComponentInParent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        menuHandler.OnDown(transform);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        menuHandler.OnEnter(transform);
        image.enabled = true;
        animator.enabled = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        menuHandler.OnExit(transform);
        image.enabled = false;
        animator.enabled = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        menuHandler.OnUp(transform);
    }
}
