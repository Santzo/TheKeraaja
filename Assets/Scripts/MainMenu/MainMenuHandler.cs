using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour, IMainMenuHandler
{


    public void OnDown(Transform trans)
    {
   
    }

    public void OnEnter(Transform trans)
    {
        Debug.Log(trans.name);
    }

    public void OnExit(Transform trans)
    {
        
    }

    public void OnUp(Transform trans)
    {
    
    }

}
