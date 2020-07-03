using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class HighScoresHandler : MonoBehaviour, IMainMenuHandler, IDragHandler, IBeginDragHandler
{
    TextMeshProUGUI keraysera;
    Slider slider;
    RectTransform entries;
    public GameObject lataus;
    public GameObject entryPrefab;
    private HighScorePool[] highScorePool;
    float separation = 73f;
    float entriesHeight;
    Vector2 oldMousePos, currentMousePos;
    private class HighScorePool
    {
        public GameObject obj;
        public HighScoreEntryPrefab prefab;
    }
    private void Awake()
    {
        keraysera = transform.Find("Keraysera").GetComponent<TextMeshProUGUI>();
        entries = transform.Find("Entries").GetChild(0).GetComponent<RectTransform>();
        entriesHeight = transform.Find("Entries").GetComponent<RectTransform>().sizeDelta.y;
        slider = GetComponentInChildren<Slider>();
        highScorePool = new HighScorePool[100];
        for (int i = 0; i < highScorePool.Length; i++)
        {
            highScorePool[i] = new HighScorePool();
            highScorePool[i].obj = Instantiate(entryPrefab, entries, true);
            highScorePool[i].obj.transform.localScale = Vector3.one;
            highScorePool[i].obj.transform.localPosition = new Vector2(0f, -i * separation);
            highScorePool[i].prefab = highScorePool[i].obj.GetComponent<HighScoreEntryPrefab>();
        }
    }
    private void OnEnable()
    {
        keraysera.text = Settings.keraysera != null ? Settings.keraysera.pokaName : "";
        Settings.keraysera.onHighScoreListLoaded = ShowHighScores;
        var updateTime = Time.realtimeSinceStartup - Settings.keraysera.timeSinceLastListUpdate;
        ShowHighScores();
        if (Settings.keraysera.highScoreList == null || Settings.keraysera.highScoreList.Count == 0)
        {
            Settings.keraysera.isHighScoreListUpdated = false;
            HighScoreManager.Instance.GetHighScores(Settings.keraysera);
        }
        else if (Settings.keraysera.highScoreList.Count > 0 && Time.realtimeSinceStartup - Settings.keraysera.timeSinceLastListUpdate > 20f)
        {
            Settings.keraysera.isHighScoreListUpdated = false;
            HighScoreManager.Instance.GetHighScores(Settings.keraysera);
        }
        lataus.SetActive(!Settings.keraysera.isHighScoreListUpdated);
    }
    private void OnDisable()
    {
        slider.onValueChanged.RemoveAllListeners();
    }
    private void ShowHighScores(NetWorkResponse res = NetWorkResponse.Success)
    {

        entries.anchoredPosition = Vector2.zero;
        if (res == NetWorkResponse.Success)
        {
            lataus.SetActive(!Settings.keraysera.isHighScoreListUpdated);
            int highScoreMax = Settings.keraysera.highScoreList.Count;
            if (highScoreMax > 7)
            {
                slider.gameObject.SetActive(true);
                slider.maxValue = Settings.keraysera.highScoreList.Count - entriesHeight / separation;
                slider.value = 0;
                slider.onValueChanged.AddListener(value => entries.anchoredPosition = new Vector2(0f, value * separation));
            }
            else
            {
                slider.value = 0;
                slider.gameObject.SetActive(false);
            }
            for (int i = 0; i < 100; i++)
            {
                if (highScoreMax <= i)
                {
                    highScorePool[i].obj.SetActive(false);
                    continue;
                }
                highScorePool[i].obj.SetActive(true);
                highScorePool[i].prefab.SetEntry(i, Settings.keraysera.highScoreList[i]);
            }
        }
        else if (res == NetWorkResponse.NoData)
        {
            lataus.GetComponent<TextMeshProUGUI>().text = "Ei tuloksia.";
        }
        else if (res == NetWorkResponse.NoConnection)
        {
            lataus.GetComponent<TextMeshProUGUI>().text = "Ei internet-yhteyttä.";
        }

    }

    public void OnDown(Transform trans)
    {

    }

    public void OnUp(Transform trans)
    {
        if (trans.name == "Takaisin")
        {
            gameObject.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentMousePos = Input.mousePosition;
        float yDelta = (currentMousePos.y - oldMousePos.y) * 0.07f;
        slider.value += yDelta;
        oldMousePos = currentMousePos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        oldMousePos = Input.mousePosition;
    }
}
