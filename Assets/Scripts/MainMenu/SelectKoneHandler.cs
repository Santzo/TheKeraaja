using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectKoneHandler : MonoBehaviour, IMainMenuHandler
{
    Vector2 startPos = new Vector2(-493f, 240.57f);
    List<SelectKone> koneet = new List<SelectKone>();
    public GameObject konePrefab;
    AsyncOperation startGame;
    int selected = 0;
    float separator;

    void Start()
    {
        separator = konePrefab.GetComponent<RectTransform>().sizeDelta.y + 15f;
        for (int i = 0; i < Settings.kerayskoneet.Length; i++)
        {
            var obj = Instantiate(konePrefab, transform, true);
            obj.transform.localScale = new Vector3(1f, 1f, 1f);
            obj.name = i.ToString();
            obj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(startPos.x, startPos.y - i * separator, 0f);

            SelectKone stats = obj.GetComponent<SelectKone>();
            stats.SetValues(Settings.kerayskoneet[i]);
            koneet.Add(stats);
        }
        UpdateSelections();
    }

    public void OnDown(Transform trans)
    {
    }

    public void OnUp(Transform trans)
    {
        if (trans.name == "Aloita")
        {
            startGame = SceneManager.LoadSceneAsync("MainGame");
            Events.onStartLoading();
            StartCoroutine(StartGame());
        }
        else
        {
            int index = int.Parse(trans.name);
            selected = index;
            UpdateSelections();
        }
    }
    public void UpdateSelections()
    {
        for (int i = 0; i < koneet.Count; i++)
        {
            koneet[i].UpdateSelection(i == selected);
        }
    }
    IEnumerator StartGame()
    {
        while (!startGame.isDone)
        {
            Events.loadProgress(startGame.progress);
            yield return null;
        }
    }
}
