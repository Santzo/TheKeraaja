using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GameObject startKerays;
    private void Awake()
    {
        startKerays = transform.Find("StartKerays").gameObject;
        startKerays.SetActive(false);
    }
    private void Start()
    {
        Events.onCloseToCollectionPoint += e => startKerays.SetActive(e);
    }
    private void OnDisable()
    {
        Events.onCloseToCollectionPoint -= e => startKerays.SetActive(e);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Events.onStartCollecting();
        Events.isPlayerCollecting = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
