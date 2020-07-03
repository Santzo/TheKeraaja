using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ValmisHandler : MonoBehaviour, IMainMenuHandler
{
    internal TextMeshProUGUI otsikko, aika;
    internal GameObject highScores;

    private void Awake()
    {
        otsikko = transform.Find("Otsikko").GetComponent<TextMeshProUGUI>();
        aika = transform.Find("Aika").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        highScores = transform.parent.Find("HighScoresPanel").gameObject;
        highScores.SetActive(false);
    }
    public void OnDown(Transform trans)
    {
    }
    public void OnUp(Transform trans)
    {
        switch (trans.name)
        {
            case "MainMenu":
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
                break;
            case "Restart":
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainGame");
                break;
            case "HighScores":
                highScores.SetActive(true);
                break;
        }
    }

}
