using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CollectingHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    TextMeshProUGUI keraaViela;
    Image[] images;
    Color oriColor;
    float tapSpeed, increaseSpeed = 0.145f;
    bool rightPressedLast = false;
    bool startedTapping = false;
    string currentPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        currentPressed = eventData.pointerEnter.name;
        if (currentPressed == "RightCircle")
        {
            images[0].color = Settings.buttonPressed;
            images[1].color = oriColor;
        }
        else
        {
            images[1].color = Settings.buttonPressed;
            images[0].color = oriColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData == null) return;
        if (!startedTapping)
        {
            tapSpeed += increaseSpeed;
            startedTapping = true;
            rightPressedLast = currentPressed == "RightCircle" ? true : false;
        }
        switch (currentPressed)
        {
            case "RightCircle":
                images[0].color = oriColor;
                if (rightPressedLast) break;
                tapSpeed += increaseSpeed;
                rightPressedLast = true;
                break;
            case "LeftCircle":
                images[1].color = oriColor;
                if (!rightPressedLast) break;
                tapSpeed += increaseSpeed;
                rightPressedLast = false;
                break;
        }
    }
    private void OnEnable()
    {
        tapSpeed = 0.5f;
        images[0].color = images[1].color = oriColor;
        startedTapping = false;
    }

    private void Awake()
    {
        keraaViela = transform.Find("KeraaViela").GetComponent<TextMeshProUGUI>();
        Events.onUpdateRemainingAmount += a => keraaViela.text = a > 0 ? $"Kerää vielä\n {a}" : "Rivi kerätty";
        images = new Image[2];
        images[0] = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        images[1] = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        oriColor = images[0].color;
    }
    private void Update()
    {
        tapSpeed = Mathf.Max(tapSpeed - (Time.deltaTime * tapSpeed * 0.5f), 1f);
        Settings.debugText.text = tapSpeed.ToString();
        Events.onTapMovementSpeed(tapSpeed);
    }
}
