﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GameObject startKerays, aloita;
    ValmisHandler valmisHandler;
    Transform uiWhileDriving, uiWhileCollecting, pokaValmis, timer;
    TextMeshProUGUI parasAika;

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
        parasAika = pokaValmis.Find("ParasAika").GetComponent<TextMeshProUGUI>();
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
        var timerText = Global.FromFloatToTime(time);
        valmisHandler.aika.text = finished ? "Aikasi oli " + timerText + "." : "Jätit tavaroita keräämättä, joten aikaasi ei hyväksytä.";

        if (!finished)
        {
            if (Settings.keraysera.userBestTime > 0f) parasAika.text = "Paras aikasi on " + Settings.keraysera.userBestTime;
            else if (Settings.keraysera.userBestTime == 0f)
            {
                HighScoreManager.Instance.FindUser(Settings.keraysera);
                Settings.keraysera.onUserBestTimeUpdated += (res, oldTime) =>
                {
                    if (oldTime > 0f) parasAika.text = "Paras aikasi on " + Global.FromFloatToTime(oldTime);
                    else parasAika.text = "Sinulla ei ole vielä aikaa tälle keräyserälle.";
                };
            }
            else if (Settings.keraysera.userBestTime < 0f)
            {
                parasAika.text = "Sinulla ei ole vielä aikaa tälle keräyserälle.";
            }
            return;
        }

        parasAika.text = "Lähetetään tietoja...";
        if (Settings.username == "")
        {
            parasAika.text = "Et ole vielä valinnut käyttäjänimeä. Aikojasi ei kirjata, ennenkuin valitset käyttäjänimen.";
            return;
        }
        Settings.keraysera.onNewTimePosted += (res, text) => parasAika.text = text;

        if (Settings.keraysera.userBestTime < 0f || Settings.keraysera.userBestTime > 0f && time < Settings.keraysera.userBestTime)
        {
            PostNewTime(time);
        }
        else if (Settings.keraysera.userBestTime == 0f)
        {
            HighScoreManager.Instance.FindUser(Settings.keraysera);
            Settings.keraysera.onUserBestTimeUpdated += (res, oldTime) => CheckForNewTime(res, time, oldTime);
        }
        else if (Settings.keraysera.userBestTime > 0f && time >= Settings.keraysera.userBestTime)
        {
            parasAika.text = "Ikävä kyllä et parantanut parasta aikaasi tälle keräyserälle. Paras aikasi tälle keräyserälle on " + Global.FromFloatToTime(Settings.keraysera.userBestTime) + ".";
        }
    }

    private void CheckForNewTime(NetWorkResponse res, float newTime, float oldTime)
    {
        if (res == NetWorkResponse.NoConnection)
        {
            parasAika.text = "Ei internet-yhteyttä. Aikaasi ei päivitetty online-tietokantaan.";
            return;
        }
        else if (res == NetWorkResponse.NoData)
        {
            PostNewTime(newTime);
        }
        else if (res == NetWorkResponse.Success)
        {
            if (newTime < oldTime) PostNewTime(newTime);
            else parasAika.text = "Ikävä kyllä et parantanut parasta aikaasi tälle keräyserälle. Paras aikasi tälle keräyserälle on " + Global.FromFloatToTime(Settings.keraysera.userBestTime) + ".";
        }
    }

    private void PostNewTime(float time)
    {
        string kone = Settings.kerayskone.name == "0" ? "Punainen" : Settings.kerayskone.name == "1" ? "Vihreä" : "Violetti";
        var score = new HighScoreEntry(Settings.username, time, kone);

        HighScoreManager.Instance.PostNewScore(score, Settings.keraysera);
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
