﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GameObject startKerays;
    Transform uiWhileDriving, uiWhileCollecting;
    private void Awake()
    {
        uiWhileDriving = GameObject.Find("UIWhileDriving").transform;
        uiWhileCollecting = GameObject.Find("UIWhileCollecting").transform;
        uiWhileCollecting.gameObject.SetActive(false);
        startKerays = uiWhileDriving.Find("StartKerays").gameObject;
        startKerays.SetActive(false);
    }
    private void Start()
    {
        Events.onCloseToCollectionPoint += e => startKerays.SetActive(e);
        Events.onStartCollecting += e => uiWhileCollecting.gameObject.SetActive(e);
        Events.onStartCollecting += e => uiWhileDriving.gameObject.SetActive(!e);
    }
    private void OnDisable()
    {
        Events.onCloseToCollectionPoint -= e => startKerays.SetActive(e);
        Events.onStartCollecting -= e => uiWhileCollecting.gameObject.SetActive(e);
        Events.onStartCollecting -= e => uiWhileDriving.gameObject.SetActive(!e);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
