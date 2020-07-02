using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeraysLista
{
    public int kaytava;
    public int paikka;
    public int maara;
    public int howManyLeft;
    public int material;
}
[System.Serializable]
public class HighScoreEntry
{
    public HighScoreEntry(string userName, float time, string kone)
    {
        this.userName = userName;
        this.time = time;
        this.kone = kone;
    }
    public string userName;
    public float time;
    public string kone;
}
public enum NetWorkResponse
{
    Success,
    NoData,
    NoConnection
}




