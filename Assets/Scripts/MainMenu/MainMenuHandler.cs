using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour, IMainMenuHandler
{
    GameObject sidePanel;
    Vector3 sidePanelPos, sidePanelOffPos;
    float panelMoveSpeed = 4500f;

    private void Awake()
    {
        sidePanel = transform.parent.Find("SidePanel").gameObject;
        sidePanelPos = sidePanel.transform.localPosition;
        sidePanel.transform.localPosition = sidePanel.transform.localPosition + Vector3.right * Screen.width;
        sidePanelOffPos = sidePanel.transform.localPosition;
    }

    public void OnDown(Transform trans)
    {

    }

    public void OnEnter(Transform trans)
    {

    }

    public void OnExit(Transform trans)
    {

    }

    public void OnUp(Transform trans)
    {
        StartCoroutine(MoveSidePanel());
        switch (trans.name)
        {
            case "Uusi":
                SceneManager.LoadScene("MainGame");
                break;
        }
    }
    IEnumerator MoveSidePanel()
    {
        sidePanel.transform.localPosition = sidePanelOffPos;
        while (sidePanel.transform.localPosition != sidePanelPos)
        {
            sidePanel.transform.localPosition = Vector2.MoveTowards(sidePanel.transform.localPosition, sidePanelPos, panelMoveSpeed * Time.deltaTime);
            yield return null;
        }
        sidePanel.transform.localPosition = sidePanelPos;
        Debug.Log("Done");
    }

}
