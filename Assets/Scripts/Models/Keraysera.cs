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
    [System.NonSerialized] public int userLeaderBoardIndex = -1;
    [System.NonSerialized] public float userBestTime = 0f;
    public Action<NetWorkResponse, float> onUserBestTimeUpdated = delegate { };
    public Action<NetWorkResponse, string> onNewTimePosted = delegate { };
    public Action<NetWorkResponse> onHighScoreListLoaded = delegate { };
    [HideInInspector] public bool isHighScoreListUpdated;
    [HideInInspector] public float timeSinceLastListUpdate;
    [System.NonSerialized] public List<HighScoreEntry> highScoreList = new List<HighScoreEntry>();
    public void ResetDelegates()
    {
        onUserBestTimeUpdated = delegate { };
        onNewTimePosted = delegate { };
        onHighScoreListLoaded = delegate { };
    }
}
