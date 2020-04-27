using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Keraysera")]
public class Keraysera : ScriptableObject
{
    public string pokaName;
    [TextArea(minLines:5, maxLines:20)]
    public string description;
    public KeraysLista[] keraysLista;
}
