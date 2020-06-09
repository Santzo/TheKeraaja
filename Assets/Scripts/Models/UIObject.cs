using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIObject : MonoBehaviour, IUIObject
{
    protected IUIHandler handler;
    protected Image image;
    protected Color originalColor;

    void Awake()
    {
        image = GetComponent<Image>();
        originalColor = image?.color ?? Color.white;
    }
    void Start()
    {
        handler = GetComponentInParent<IUIHandler>();
        if (handler == null)
        {
            Debug.Log("UI Handler not found! " + name);
            Destroy(gameObject);
        }
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        handler.OnDown(transform);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        handler.OnUp(transform);
    }


}
