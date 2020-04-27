using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HighScoresHandler : MonoBehaviour, IMainMenuHandler, IDragHandler, IBeginDragHandler
{
    TextMeshProUGUI keraysera;
    Slider slider;
    RectTransform entries;
    public GameObject entryPrefab;
    float separation = 73f;
    float entriesHeight;
    Vector2 oldMousePos, currentMousePos;

    private void Awake()
    {
        keraysera = transform.Find("Keraysera").GetComponent<TextMeshProUGUI>();
        entries = transform.Find("Entries").GetChild(0).GetComponent<RectTransform>();
        entriesHeight = transform.Find("Entries").GetComponent<RectTransform>().sizeDelta.y;
        slider = GetComponentInChildren<Slider>();
    }
    void Start()
    {
        for (int i = 0; i < 57; i++)
        {
            HighScoreEntry entry = new HighScoreEntry { id = i.ToString(), kone = "Punainen", time = 157.1689f, userName = $"User{i}" };
            HighScoreManager.Instance.currentHighscores.Add(entry);
            var obj = Instantiate(entryPrefab, entries, true);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = new Vector2(0f, -i * separation);
            HighScoreEntryPrefab hs = obj.GetComponent<HighScoreEntryPrefab>();
            hs.SetEntry(i, entry);
        }
        slider.maxValue = HighScoreManager.Instance.currentHighscores.Count - entriesHeight / separation;
        slider.value = 0;
        slider.onValueChanged.AddListener(value => entries.anchoredPosition = new Vector2(0f, value * separation));
    }
    private void OnEnable()
    {
        keraysera.text = Settings.keraysera != null ? Settings.keraysera.pokaName : "";
    }
    public void OnDown(Transform trans)
    {

    }

    public void OnUp(Transform trans)
    {
        if (trans.name == "Takaisin")
        {
            Debug.Log(Time.time);
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
