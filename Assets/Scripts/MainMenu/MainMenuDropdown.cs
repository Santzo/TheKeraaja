using System;
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
    Color oriColor;
    Image background;
    List<GameObject> objList = new List<GameObject>();
    bool closed = true;

    public void Awake()
    {
        dropHandler = GetComponentInParent<IDropDownHandler>();
        bg = transform.GetComponent<RectTransform>();
        border = transform.Find("Border").GetComponent<RectTransform>();
        textLabel = transform.Find("Label").gameObject;
        label = textLabel.GetComponentInChildren<TextMeshProUGUI>();
        borderHeight = border.GetComponent<RectTransform>().sizeDelta.y - 6f;
        bgHeight = textLabel.GetComponent<RectTransform>().sizeDelta.y;
        background = textLabel.GetComponent<Image>();
        oriColor = background.color;
        int value = (int)typeof(Settings).GetField(name.ToLower()).GetValue(null);
        label.text = options[value];
        textHeight = textLabel.GetComponent<RectTransform>().sizeDelta.y - 3f;
        Debug.Log(Screen.currentResolution);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var pressed = eventData.pointerEnter.gameObject;
        pressed.GetComponentInParent<Image>().color = Settings.buttonPressed;
    }

    void SetHeight(int height)
    {
        if (objList.Count > 0)
        {
            objList.ForEach(obj => Destroy(obj));
            objList = new List<GameObject>();
        }
        //bg.sizeDelta = new Vector2(bg.sizeDelta.x, bgHeight * height);
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
                    obj.transform.localScale = textLabel.transform.localScale;
                    obj.transform.SetAsFirstSibling();
                    obj.GetComponent<Image>().color = oriColor;
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = options[i];
                    objList.Add(obj);
                    count++;
                }
            }
        }
        else
        {
            var option = Array.IndexOf(options, text.text);
            Settings.SaveSetting(name.ToLower(), option);
            label.text = text.text;
            SetHeight(1);
        }
        closed = !closed;
        background.color = oriColor;
    }

}
