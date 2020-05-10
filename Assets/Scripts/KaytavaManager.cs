using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KaytavaManager : MonoBehaviour
{
    TextMeshProUGUI timer, seuraava, etaisyys, fps;
    Transform indicator, player;
    Rigidbody rb;
    Transform[] kaytavat;
    float time = 0f;
    Vector3 indicatorOri;
    public static Vector3 currentKeraysTarget;
    Canvas canvas;
    float zSpace = 1.514f;
    float kaytavaVali = 9.0f;
    float extraJump = 0.108f;
    float hyllyvali = 28.98f;
    float osoiteVali = 3f;
    int kaytava = 1;
    int hyllyPaikka = 124;
    int rivi = 0;
    bool isCloseToKeraysPoint = false;
    float acceptedVel = 1f, acceptedDist = 2f;
    KeraysLista[] keraysera;

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
        keraysera = Settings.kerayserat[0].keraysLista;
        timer = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        fps = GameObject.Find("FPS").GetComponent<TextMeshProUGUI>();
        player = GameObject.Find("Kerayskone").transform;
        rb = player.GetComponent<Rigidbody>();
        seuraava = GameObject.Find("Kerayspaikka").GetComponent<TextMeshProUGUI>();
        etaisyys = GameObject.Find("Etaisyys").GetComponent<TextMeshProUGUI>();
        canvas = GameObject.Find("UI").GetComponent<Canvas>();
        indicator = GameObject.Find("Indicator").transform;
        indicatorOri = indicator.transform.position;
        InitializeNumbers();
        //timer.transform.position = new Vector2(timer.transform.position.x, Screen.height - 70f * canvas.scaleFactor);
        SeuraavaRivi();
    }

    private void SeuraavaRivi()
    {
        KeraysLista temp = keraysera[rivi];
        PlaceIndicator(temp.kaytava, temp.paikka);
        rivi++;
        if (rivi >= keraysera.Length)
        {
            Debug.Log("Poka valmis");
            rivi = keraysera.Length - 1;
        }
    }

    private void Update()
    {
        UpdateTime();
        var dist = UpdateDistance();
        //var xDistance = currentKeraysTarget.x - player.position.x;
        //Debug.Log(xDistance);
        var a = Vector3.Cross(player.forward, currentKeraysTarget);
        var test = Vector3.Dot(a, player.up);
        Debug.Log(test);
        if (rb.velocity.magnitude <= acceptedVel && dist <= acceptedDist && !Events.isPlayerCloseToCollectionPoint)
        {
            Events.isPlayerCloseToCollectionPoint = true;
            Events.onCloseToCollectionPoint(true);
        }
        if (rb.velocity.magnitude > acceptedVel || dist >= acceptedDist && Events.isPlayerCloseToCollectionPoint)
        {
            Events.isPlayerCloseToCollectionPoint = false;
            Events.onCloseToCollectionPoint(false);
        }
        if (Time.frameCount % 10 == 0)
            UpdateDistanceText(dist);
    }

    private float UpdateDistance()
    {
        Vector2 pl = new Vector2(player.position.x, player.position.z);
        Vector2 dest = new Vector2(indicator.position.x, indicator.position.z);
        return Vector2.Distance(pl, dest);

    }
    private void UpdateDistanceText(float dist)
    {
        etaisyys.text = string.Format("{0:0.0}m", dist);

        fps.text = (1f / Time.deltaTime).ToString("F1");
    }

    private void UpdateTime()
    {
        time += Time.deltaTime;
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.Floor(time - minutes * 60);
        float milliseconds = Mathf.Floor((time % 1) * 10f);
        timer.text = string.Format("{0:00}:{1:00},{2:0}", minutes, seconds, milliseconds);

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
                                num = 300 + (32 - (a * 2 + 2));
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
        seuraava.text = $"0{kaytava}-{osoite}";
        Vector3 pos = indicator.transform.position;
        int hylly = osoite / 100 - 1;
        float xOffset, offset;
        osoite = osoite % 100;
        if (kaytava % 2 != 0)
        {
            if (osoite % 2 != 0)
            {
                int kerroin = (osoite - 1) / 2;
                int extra = kerroin / 3;
                xOffset = (kaytava - 1) * kaytavaVali;
                offset = hylly * hyllyvali + kerroin * zSpace + extra * extraJump;
            }
            else
            {
                int kerroin = (osoite - 2) / 2;
                int extra = kerroin / 3;
                xOffset = (kaytava - 1) * kaytavaVali - osoiteVali;
                offset = hylly * hyllyvali + kerroin * zSpace + extra * extraJump;
            }
        }
        else
        {
            if (osoite % 2 == 0)
            {
                int kerroin = (30 - osoite) / 2;
                int extra = kerroin / 3;
                xOffset = (kaytava - 1) * kaytavaVali;
                offset = (2 - hylly) * hyllyvali + kerroin * zSpace + extra * extraJump;
            }
            else
            {
                int kerroin = (29 - osoite) / 2;
                int extra = kerroin / 3;
                xOffset = (kaytava - 1) * kaytavaVali - osoiteVali;
                offset = (2 - hylly) * hyllyvali + kerroin * zSpace + extra * extraJump;
            }
        }
        indicator.transform.position = new Vector3(indicatorOri.x + xOffset, indicatorOri.y, indicatorOri.z + offset);
        var multi = kaytava * osoite % 2 != 0 ? indicator.transform.right : -indicator.transform.right;
        currentKeraysTarget = indicator.transform.position + multi;
        Debug.DrawLine(indicator.transform.position, currentKeraysTarget, Color.cyan, 10f);
    }
}
