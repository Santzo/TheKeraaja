using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KaytavaManager : MonoBehaviour
{
    TextMeshProUGUI timer, seuraava, etaisyys;
    Transform indicator, player;
    Rigidbody rb;
    Transform[] kaytavat, aktiivit;
    public Material[] laatikot;
    LODGroup[] lodGroup;
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
    public KeraysLista[] keraysera;

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
        lodGroup = FindObjectsOfType<LODGroup>();
        for (int a = 0; a < lodGroup.Length; a++)
        {
            var lod = lodGroup[a].GetLODs();
            lod[0] = new LOD(0.15f, lod[0].renderers);
            lodGroup[a].SetLODs(lod);
            lodGroup[a].RecalculateBounds();

        }
        var aktiivit = transform.GetAllChildren("Laatikot");
        for (int i = 0; i < aktiivit.Length; i+=2)
        {
            var rnd = UnityEngine.Random.Range(0, laatikot.Length);
            aktiivit[i].GetComponent<MeshRenderer>().sharedMaterial = laatikot[rnd];
            aktiivit[i + 1].GetComponent<MeshRenderer>().sharedMaterial = laatikot[rnd];

            //aktiivit[i].GetComponent<MeshRenderer>().sharedMaterial = laatikot[0];
        }
        keraysera = Settings.kerayserat[0].keraysLista;
        Events.seuraavaRivi += SeuraavaRivi;
        for (int i = 0; i < keraysera.Length; i++)
        {
            keraysera[i].howManyLeft = keraysera[i].maara;
        }
        timer = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        player = GameObject.Find("Kerayskone").transform;
        rb = player.GetComponent<Rigidbody>();
        seuraava = GameObject.Find("Kerayspaikka").GetComponent<TextMeshProUGUI>();
        etaisyys = GameObject.Find("Etaisyys").GetComponent<TextMeshProUGUI>();
        canvas = GameObject.Find("UI").GetComponent<Canvas>();
        indicator = GameObject.Find("Indicator").transform;
        indicatorOri = indicator.transform.position;
        InitializeNumbers();
        SeuraavaRivi();
    }

    private void SeuraavaRivi()
    {
        Events.currentRivi = keraysera[rivi];
        PlaceIndicator(keraysera[rivi].kaytava, keraysera[rivi].paikka);
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
        currentKeraysTarget = indicator.transform.position + multi * 0.8f;
    }
}
