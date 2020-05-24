using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RullakkoController : MonoBehaviour
{
    Transform[] laatikot;
    MeshRenderer[] renderers;
    private int boxIndex = 0;
    private void Awake()
    {
        laatikot = GetComponentsInChildren<Transform>().Where(go => go != transform).ToArray();
        renderers = GetComponentsInChildren<MeshRenderer>().Where(go => go.transform != transform).ToArray();
        laatikot.ForEach(a => a.gameObject.SetActive(false));
    }
    public void ActivateBox()
    {
        laatikot[boxIndex].gameObject.SetActive(true);
        boxIndex++;
    }
}
