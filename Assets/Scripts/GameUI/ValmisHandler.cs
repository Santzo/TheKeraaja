using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValmisHandler : MonoBehaviour, IMainMenuHandler
{
    public void OnDown(Transform trans)
    {
    }

    public void OnUp(Transform trans)
    {
        Debug.Log(trans.name);
    }

}
