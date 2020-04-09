using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuDropdown : MonoBehaviour, IUIObject
{
    GameObject textLabel;
    public string[] options;
    IDropDownHandler dropHandler;
    float bgHeight, borderHeight, textHeight;
    RectTransform bg, border;
    TextMeshProUGUI label;
    List<GameObject> objList = new List<GameObject>();
    bool closed = true;

    public void Awake()
    {
        dropHandler = GetComponentInParent<IDropDownHandler>();
        bg = transform.GetComponent<RectTransform>();
        border = transform.Find("Border").GetComponent<RectTransform>();
        textLabel = transform.Find("Label").gameObject;
        label = textLabel.GetComponent<TextMeshProUGUI>();
        borderHeight = border.GetComponent<RectTransform>().sizeDelta.y;
        bgHeight = bg.GetComponent<RectTransform>().sizeDelta.y;
        label.text = options[0];
        textHeight = textLabel.GetComponent<RectTransform>().sizeDelta.y;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        var text = eventData.pointerEnter.gameObject?.GetComponent<TextMeshProUGUI>();
        int count = 1;
        if (closed)
        {
            SetHeight(options.Length);
            for (int i = 0; i < options.Length; i++)
            {
                if (options[i] != label.text)
                {
                    GameObject obj = Instantiate(textLabel);
                    obj.transform.SetParent(transform, true);
                    obj.transform.localPosition = new Vector2(textLabel.transform.localPosition.x, textLabel.transform.localPosition.y - textHeight * count);
                    obj.GetComponent<TextMeshProUGUI>().text = options[i];
                    objList.Add(obj);
                    count++;
                }
            }
        }
        else
        {
            label.text = text.text;
            SetHeight(1);
        }
        closed = !closed;
    }

    void SetHeight(int height)
    {
        if (objList.Count > 0)
        {
            objList.ForEach(obj => Destroy(obj));
            objList = new List<GameObject>();
        }
        bg.sizeDelta = new Vector2(bg.sizeDelta.x, bgHeight * height);
        border.sizeDelta = new Vector2(border.sizeDelta.x, borderHeight * height);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

}
