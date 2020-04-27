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
    TextMeshProUGUI kuvausTeksti;
    IMainMenuHandler mainMenu;

    public void OnDown(Transform trans)
    {
        if (trans.name == "Kerays")
        {
            images.ForEach(img => img.color = oriColor);
            int valinta = Settings.kerayserat.Length - 1 - trans.GetSiblingIndex();
            kuvausTeksti.text = Settings.kerayserat[valinta].description;
            Settings.keraysera = Settings.kerayserat[valinta];
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
        float height = label.GetComponent<RectTransform>().sizeDelta.y;

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
        }
        oriColor = images[0].color;
        images[0].color = Settings.buttonPressed;
        Settings.keraysera = Settings.kerayserat[0];
        kuvausTeksti.text = Settings.keraysera.description;
        border.sizeDelta = new Vector2(border.sizeDelta.x, height * Settings.kerayserat.Length * 2f + 6f);
    }
}
