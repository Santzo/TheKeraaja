using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GameObject startKerays, aloita;
    ValmisHandler valmisHandler;

    Transform uiWhileDriving, uiWhileCollecting, pokaValmis, timer;
    private void Awake()
    {
        Time.timeScale = 0f;
        uiWhileDriving = GameObject.Find("UIWhileDriving").transform;
        uiWhileCollecting = GameObject.Find("UIWhileCollecting").transform;
        aloita = transform.Find("Aloita").gameObject;
        pokaValmis = transform.Find("Valmis");
        valmisHandler = pokaValmis.GetComponent<ValmisHandler>();
        uiWhileCollecting.gameObject.SetActive(false);
        startKerays = uiWhileDriving.Find("StartKerays").gameObject;
        startKerays.SetActive(false);
    }
    private void Start()
    {
        Events.onCloseToCollectionPoint += e => startKerays.SetActive(e);
        Events.onStartCollecting += e => uiWhileCollecting.gameObject.SetActive(e);
        Events.onStartCollecting += e => uiWhileDriving.gameObject.SetActive(!e);
        Events.onPlayerCrossedFinishLine += FinishLineCrossed;
        pokaValmis.gameObject.SetActive(false);
    }

    private void FinishLineCrossed(bool finished, float time)
    {
        Time.timeScale = 0f;
        uiWhileDriving.gameObject.SetActive(false);
        uiWhileCollecting.gameObject.SetActive(false);
        KaytavaManager.Instance.timer.gameObject.SetActive(false);
        pokaValmis.gameObject.SetActive(true);
        valmisHandler.otsikko.text = finished ? "Keräyserä valmis" : "Keräyserä kesken";

        float minutes = Mathf.Floor(time / 60f);
        float seconds = Mathf.Floor(time - minutes * 60f);
        float milliseconds = Mathf.Floor((time % 1f) * 10f);
        var timerText = $"{(int)time / 60}:{(time) % 60:00.000}";
        valmisHandler.aika.text = finished ? "Aikasi oli " + timerText + "." : "Jätit tavaroita keräämättä, joten aikaasi ei hyväksytä.";

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
        if (eventData.pointerEnter.name == "Aloita")
        {
            Time.timeScale = 1f;
            aloita.SetActive(false);
        }
    }
}
