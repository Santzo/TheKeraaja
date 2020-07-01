using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Keraysera")]
public class Keraysera : ScriptableObject
{
    public string pokaName;
    [TextArea(minLines: 5, maxLines: 20)]
    public string description;
    public KeraysLista[] keraysLista;
    private float _userBestTime = -1f;
    public Action<float> onUserBestTimeUpdated = delegate { };
    public Action<string> onNewTimePosted = delegate { };
    public float userBestTime
    {
        get
        {
            return _userBestTime;
        }
        set
        {
            if (value == _userBestTime) return;
            _userBestTime = value;
            onUserBestTimeUpdated(_userBestTime);
        }
    }
    public void ResetDelegates()
    {
        onUserBestTimeUpdated = delegate { };
        onNewTimePosted = delegate { };
    }
}
