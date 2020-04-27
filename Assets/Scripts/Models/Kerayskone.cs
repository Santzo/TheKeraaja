using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Kerayskone")]
public class Kerayskone : ScriptableObject
{
    public GameObject keraysKone;
    [Range(1, 10)]
    public float nopeus, kiihtyvyys, kaantyvyys, jarrutus;
}
