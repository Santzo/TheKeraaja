using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KaytavaManager : MonoBehaviour
{
    TextMeshProUGUI timer;
    Transform indicator;
    Transform[] kaytavat;
    float time = 0f;
    Vector3 indicatorOri;
    Canvas canvas;
    float zSpace = 1.513f;
    float extraJump = 0.106f;
    float hyllyvali = 28.98f;
    int kaytava = 1;
    int hyllyPaikka = 124;

    private void Awake()
    {
        kaytavat = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            kaytavat[i] = transform.GetChild(i);
        }
    }
    private void Start()
    {
        timer = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        canvas = GameObject.Find("UI").GetComponent<Canvas>();
        indicator = GameObject.Find("Indicator").transform;
        indicatorOri = indicator.transform.position;
        InitializeNumbers();
        //timer.transform.position = new Vector2(timer.transform.position.x, Screen.height - 70f * canvas.scaleFactor);
        PlaceIndicator(1, 326);
    }
    private void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        time += Time.deltaTime;
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.Floor(time - minutes * 60);
        float milliseconds = Mathf.Floor((time % 1)* 10f);
        timer.text = string.Format("{0:00}:{1:00}.{2:0}", minutes, seconds, milliseconds);
    }

    private void InitializeNumbers()
    {
        for (int i = 0; i < kaytavat.Length; i++)
        {
            TextMeshPro[] numbers = kaytavat[i].GetComponentsInChildren<TextMeshPro>();
            int half = numbers.Length / 2;
            int third = half / 3;
            int iterations = numbers.Length / third;

            if (i % 2 == 0)
            {
                for (int it = 0; it < iterations; it++)
                {
                    for (int a = 0; a < third; a++)
                    {
                        int num = 0;
                        switch (it)
                        {
                            case 0:
                                num = 100 + a * 2 + 1;
                                break;
                            case 1:
                                num = 200 + a * 2 + 1;
                                break;
                            case 2:
                                num = 300 + a * 2 + 1;
                                break;
                            case 3:
                                num = 100 + (32 - (a * 2 + 2));
                                break;
                            case 4:
                                num = 200 + (32 - (a * 2 + 2));
                                break;
                            case 5:
                                num = 300 + (32 - (a * 2 + 2));
                                break;
                        }
                        numbers[a + (it * third)].text = "0" + (i + 1) + "-" + num;
                    }
                }
            }
            else 
            {
                for (int it = 0; it < iterations; it++)
                {
                    for (int a = 0; a < third; a++)
                    {
                        int num = 0;
                        switch (it)
                        {
                            case 0:
                                num = 300 + (32- (a * 2 + 2));
                                break;
                            case 1:
                                num = 200 + (32 - (a * 2 + 2));
                                break;
                            case 2:
                                num = 100 + (32 - (a * 2 + 2));
                                break;
                            case 3:
                                num = 300 + a * 2 + 1;
                                break;
                            case 4:
                                num = 200 + a * 2 + 1;
                                break;
                            case 5:
                                num = 100 + a * 2 + 1;
                                break;
                        }
                        numbers[a + (it * third)].text = "0" + (i + 1) + "-" + num;
                    }
                }
            }
        }
    }

    void PlaceIndicator(int kaytava, int osoite)
    {
        Vector3 pos = indicator.transform.position;
        int hylly = osoite / 100 - 1;
        osoite = osoite % 100;
        if (osoite % 2 != 0)
        {
            int kerroin = (osoite - 1) / 2;
            int extra = kerroin / 3;
            float offset = hylly * hyllyvali + kerroin * zSpace + extra * extraJump;
            indicator.transform.position = new Vector3(indicatorOri.x, indicatorOri.y, indicatorOri.z + offset);
        }
        else
        {
            int kerroin = (osoite - 2) / 2;
            int extra = kerroin / 3;
            float offset = hylly * hyllyvali + kerroin * zSpace + extra * extraJump;
            indicator.transform.position = new Vector3(indicatorOri.x - 3.9f, indicatorOri.y, indicatorOri.z + offset);
        }
    }
}
