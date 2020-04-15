using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoresHandler : MonoBehaviour, IMainMenuHandler
{
    TextMeshProUGUI keraysera;
    
    private void Awake()
    {
        keraysera = transform.Find("Keraysera").GetComponent<TextMeshProUGUI>();
        
    }
    private void OnEnable()
    {
        Debug.Log(keraysera);
        keraysera.text = Settings.keraysera != null ? Settings.keraysera.pokaName : "";
    }
    public void OnDown(Transform trans)
    {

    }

    public void OnUp(Transform trans)
    {
        if (trans.name == "Takaisin")
            gameObject.SetActive(false);
    }


}
