using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UusiKerayseraHandler : MonoBehaviour, IMainMenuHandler
{
    public GameObject label;
    Color oriColor;
    GameObject valintaLaatikko, highScores;
    RectTransform border;
    List<Image> images = new List<Image>();
    TextMeshProUGUI kuvausTeksti, parasAika;
    IMainMenuHandler mainMenu;

    public void OnDown(Transform trans)
    {
        if (trans.name == "Kerays")
        {
            UpdateKeraysEraStats(trans);
        }
    }
    public void UpdateKeraysEraStats(Transform trans = null)
    {
        Settings.keraysera.onUserBestTimeUpdated = delegate { };

        if (trans != null)
        {
            images.ForEach(img => img.color = oriColor);
        }

        int valinta = trans != null ? Settings.kerayserat.Length - 1 - trans.GetSiblingIndex() : 0;

        kuvausTeksti.text = Settings.kerayserat[valinta].description;
        Settings.keraysera = Settings.kerayserat[valinta];

        if (Settings.username == "")
        {
            parasAika.text = "Et ole vielä valinnut käyttäjänimeä. Aikojasi ei kirjata, ennenkuin valitset käyttäjänimen";
            return;
        }
        var connection = Settings.keraysera.userBestTime == -2f ? NetWorkResponse.NoConnection :
                            Settings.keraysera.userBestTime == -1f ? NetWorkResponse.NoData :
                            NetWorkResponse.Success;
        parasAika.text = ReturnParasAikaText(connection, Settings.keraysera.userBestTime);
        Settings.keraysera.onUserBestTimeUpdated = (res, time) =>
        {
            parasAika.text = ReturnParasAikaText(res, time);
            Settings.keraysera.userBestTime = time;
        };
        if (Settings.keraysera.userBestTime == 0f || connection == NetWorkResponse.NoConnection)
        {
            HighScoreManager.Instance.FindUser(Settings.keraysera);
        }
    }
    public string ReturnParasAikaText(NetWorkResponse res, float time)
    {
        switch (res)
        {
            case NetWorkResponse.NoData:
                return "Sinulla ei ole vielä aikaa tälle keräyserälle.";
            case NetWorkResponse.NoConnection:
                return "Internet-yhteyttä ei löytynyt.";
            default:
                return time == 0f ? "Haetaan aikaa..." : "Paras aikasi on: " + Global.FromFloatToTime(time);
        }
    }
    public void OnUp(Transform trans)
    {
        if (trans.name == "HighScores")
        {
            highScores.SetActive(true);
        }
        else if (trans.name == "Seuraava")
        {
            mainMenu.OnUp(null);
        }

    }

    void Start()
    {
        //SceneManager.LoadScene("MainGame");
        mainMenu = transform.parent.parent.Find("MainMenuHandler").GetComponent<IMainMenuHandler>();
        highScores = transform.Find("HighScoresPanel").gameObject;
        highScores.SetActive(false);
        valintaLaatikko = transform.Find("Valinta").gameObject;
        border = valintaLaatikko.transform.Find("Border").GetComponent<RectTransform>();
        kuvausTeksti = transform.Find("Kuvaus").GetComponentInChildren<TextMeshProUGUI>();
        parasAika = transform.Find("ParasAika").GetComponent<TextMeshProUGUI>();
        float height = label.GetComponent<RectTransform>().sizeDelta.y;
        float width = label.GetComponent<RectTransform>().sizeDelta.x;
        for (int i = 0; i < Settings.kerayserat.Length; i++)
        {
            var obj = Instantiate(label, valintaLaatikko.transform);
            obj.transform.SetParent(valintaLaatikko.transform, true);
            obj.name = "Kerays";
            obj.transform.localPosition = new Vector2(0f, 0f - height * i);
            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
            images.Add(obj.GetComponent<Image>());
            text.text = Settings.kerayserat[i].pokaName;
            obj.transform.SetAsFirstSibling();
            Settings.kerayserat[i].userBestTime = 0f;
        }
        oriColor = images[0].color;
        images[0].color = Settings.buttonPressed;
        Settings.keraysera = Settings.kerayserat[0];
        kuvausTeksti.text = Settings.keraysera.description;
        border.sizeDelta = new Vector2(width * 2f + 6f, height * Settings.kerayserat.Length * 2f + 6f);
        UpdateKeraysEraStats();
    }
}
