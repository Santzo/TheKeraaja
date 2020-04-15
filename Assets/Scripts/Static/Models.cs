using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeraysLista
{
    public int kaytava;
    public int paikka;
    public int maara;
}

public class HighScoreEntry
{
    public string userName;
    public float time;
    public string kone;
    public string id;
}

public class KeraysKone
{
    public float speed;
    public float acceleration;
    public float turnRate;
    public float braking;
}