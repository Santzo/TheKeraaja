using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KaytavaManager : MonoBehaviour
{
    internal TextMeshProUGUI timer, seuraava, etaisyys;
    Transform indicator, player;
    Rigidbody rb;
    Transform[] kaytavat, aktiivit;
    public Material[] laatikotMaterials;
    LODGroup[] lodGroup;
    public float time = 0f;
    Vector3 indicatorOri, finishFlag;
    public static Vector3 currentKeraysTarget;
    Canvas canvas;
    float zSpace = 1.514f;
    float kaytavaVali = 9.0f;
    float extraJump = 0.108f;
    float hyllyvali = 28.98f;
    float osoiteVali = 3f;
    int kaytava = 1;
    int hyllyPaikka = 124;
    public int rivi = 0;
    bool isCloseToKeraysPoint = false;
    float acceptedVel = 1f, acceptedDist = 2f;
    public KeraysLista[] keraysLista;
    public static KaytavaManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        kaytavat = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            kaytavat[i] = transform.GetChild(i);
        }
        Events.ResetEventDelegates();
        Settings.ResetKeraysEraDelegates();
    }
    private void Start()
    {

        Light mainLight = FindObjectOfType<Light>();
        mainLight.shadows = Settings.shadows == 0 ? LightShadows.Soft : LightShadows.None;
        keraysLista = Settings.keraysera?.keraysLista ?? Settings.kerayserat[0].keraysLista;
        finishFlag = GameObject.Find("FinishFlag").transform.position;
        ApplyLodSettings();
        RandomizeMaterials();
        Events.seuraavaRivi += SeuraavaRivi;
        for (int i = 0; i < keraysLista.Length; i++)
        {
            keraysLista[i].howManyLeft = keraysLista[i].maara;
        }
        timer = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        player = GameObject.Find("Kerayskone").transform;
        rb = player.GetComponent<Rigidbody>();
        seuraava = GameObject.Find("Kerayspaikka").GetComponent<TextMeshProUGUI>();
        etaisyys = GameObject.Find("Etaisyys").GetComponent<TextMeshProUGUI>();
        canvas = GameObject.Find("MainUI").GetComponent<Canvas>();
        indicator = GameObject.Find("Indicator").transform;
        indicatorOri = indicator.transform.position;
        InitializeNumbers();
        SeuraavaRivi();
    }

    private void ApplyLodSettings()
    {
        lodGroup = FindObjectsOfType<LODGroup>();
        for (int a = 0; a < lodGroup.Length; a++)
        {
            var lod = lodGroup[a].GetLODs();
            lod[0] = new LOD(0.15f, lod[0].renderers);
            lodGroup[a].SetLODs(lod);
            lodGroup[a].RecalculateBounds();

        }
    }

    private void RandomizeMaterials()
    {
        var aktiivit = transform.GetAllChildren("Laatikot");
        int[] materiaalit = new int[aktiivit.Length / 2];
        int matIndex = 0;
        for (int i = 0; i < aktiivit.Length; i += 2)
        {
            var rnd = UnityEngine.Random.Range(0, laatikotMaterials.Length);
            materiaalit[matIndex] = rnd;
            matIndex++;
            aktiivit[i].GetComponent<MeshRenderer>().sharedMaterial = laatikotMaterials[rnd];
            aktiivit[i + 1].GetComponent<MeshRenderer>().sharedMaterial = laatikotMaterials[rnd];
        }
        for (int i = 0; i < keraysLista.Length; i++)
        {
            int a = KeraysEraMatIndex(keraysLista[i].kaytava, keraysLista[i].paikka);
            keraysLista[i].material = materiaalit[a];
        }
    }
    int KeraysEraMatIndex(int kaytava, int osoite)
    {
        int kaytavaMulti = (kaytava - 1) * 30;
        int osoiteAdd = OsoiteToIndex(osoite);
        int finalIndex = kaytavaMulti + osoiteAdd;
        return finalIndex;
    }
    int OsoiteToIndex(int osoite)
    {
        int puoliAdd = osoite % 2 != 0 ? 0 : 15;
        int vyohykeAdd = osoite < 200 ? 0 : osoite < 300 ? 5 : 10;
        int divider = osoite < 200 ? 100 : osoite < 300 ? 200 : 300;
        int a = osoite % divider;
        float b = a / 6.05f;
        int osoiteAdd = Mathf.FloorToInt(b);
        return puoliAdd + vyohykeAdd + osoiteAdd;
    }
    private void SeuraavaRivi()
    {
        if (rivi >= keraysLista.Length)
        {
            Debug.Log("Poka valmis");
            Events.pokaValmis = true;
            PlaceIndicator();
            return;
        }
        Events.currentRivi = keraysLista[rivi];
        PlaceIndicator(keraysLista[rivi].kaytava, keraysLista[rivi].paikka);
        rivi++;
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
        Vector2 pl = new Vector2(Events.currentRullakko.position.x, Events.currentRullakko.position.z);
        Vector2 dest = !Events.pokaValmis ? new Vector2(indicator.position.x, indicator.position.z)
                                            : new Vector2(finishFlag.x, finishFlag.y);
        return Vector2.Distance(pl, dest);

    }
    private void UpdateDistanceText(float dist)
    {
        etaisyys.text = string.Format("{0:0.0}m", dist);
    }

    private void UpdateTime()
    {
        time += Time.deltaTime;
        timer.text = $"{(int)time / 60:00}:{time % 60:00.0}";
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

    void PlaceIndicator(int kaytava = 0, int osoite = 0)
    {
        seuraava.text = !Events.pokaValmis ? $"0{kaytava}-{osoite}" : "Aja maaliin";
        if (Events.pokaValmis)
        {
            indicator.gameObject.SetActive(false);
            return;
        }

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
