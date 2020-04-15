using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour, IMainMenuHandler
{
    GameObject sidePanel, quitPanel, asetukset, uusi;
    public TMP_InputField nameField;
    Vector3 sidePanelPos, sidePanelOffPos;
    float panelMoveSpeed = 4500f;

    private void Awake()
    {
        sidePanel = transform.parent.Find("SidePanel").gameObject;

        asetukset = sidePanel.transform.Find("Asetukset").gameObject;
        uusi = sidePanel.transform.Find("Uusi").gameObject;
        uusi.SetActive(false);
        asetukset.SetActive(false);
        quitPanel = transform.parent.Find("QuitPanel").gameObject;
        sidePanelPos = sidePanel.transform.localPosition;
        sidePanel.transform.localPosition = sidePanel.transform.localPosition + Vector3.right * Screen.width;
        sidePanelOffPos = sidePanel.transform.localPosition;
        nameField.onSubmit.AddListener(name => Settings.NameChange(name));
        nameField.text = Settings.username;
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

    public void OnDisable()
    {
        nameField.onSubmit.RemoveAllListeners();
    }
    public void OnUp(Transform trans)
    {
        if (trans.name=="Poistu")
        {
            quitPanel.SetActive(true);
            return;
        }
        StartCoroutine(MoveSidePanel(trans.name));
      
    }
    IEnumerator MoveSidePanel(string panelName)
    {
        asetukset.SetActive(panelName == "Asetukset");
        uusi.SetActive(panelName == "Uusi");
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
