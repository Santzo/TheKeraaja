using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    private bool _scoresLoaded;
    string id;
    private static HighScoreManager _instance;
    public float lastRefresh = 0f;
    private float tokenLastFetched, tokenFetchUpdateLimit = 3000f;
    const string uri = "https://thekeraaja.firebaseio.com/";
    public Action<bool> OnScoresLoaded = delegate { };
    public List<HighScoreEntry> currentHighscores = new List<HighScoreEntry>();

    private void Awake() => id = SystemInfo.deviceUniqueIdentifier;
    public void Start() => StartCoroutine(GetAuth());

    public static HighScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {

                var go = new GameObject();
                _instance = go.AddComponent<HighScoreManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public void GetHighScores(Keraysera era) => StartCoroutine(GetScores(era));

    public void PostNewScore(HighScoreEntry score, Keraysera era) => StartCoroutine(PostScore(score, era));

    public void FindUser(Keraysera era) => StartCoroutine(_FindUser(era));

    private IEnumerator PostScore(HighScoreEntry score, Keraysera era)
    {
        while (Settings.token == "")
            yield return null;
        var newScore = JsonUtility.ToJson(score);
        using (var www = UnityWebRequest.Put(uri + era.pokaName + "/" + id + ".json?auth=" + Settings.token, newScore))
        {
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(newScore));
            www.timeout = 5;
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                if (Time.realtimeSinceStartup - tokenLastFetched > tokenFetchUpdateLimit)
                    StartCoroutine(GetAuth());
                era.onNewTimePosted(NetWorkResponse.NoConnection, "Ei internet-yhteyttä. Aikaasi ei päivitetty online-tietokantaan.");
            }
            else
            {
                era.onNewTimePosted(NetWorkResponse.Success, "Aikasi on päivitetty online-tietokantaan.");
                era.userBestTime = score.time;
            }
        }
    }

    private IEnumerator GetAuth()
    {
        Settings.token = "";
        var userData = "{'returnSecureToken': 'true'}";
        var apiKey = Resources.Load<TextAsset>("Keraysera")?.text;
        var www = UnityWebRequest.Post("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + apiKey, userData);
        www.SetRequestHeader("Content-Type", "application/json");
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(userData));
        www.timeout = 5;
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Settings.token = "NotValid";
            yield return false;
        }
        else
        {
            string result = www.downloadHandler.text;
            var aa = JsonUtility.FromJson<IdToken>(result);
            Settings.token = aa.idToken;
            tokenLastFetched = Time.realtimeSinceStartup;
            yield return true;
        }
    }

    private IEnumerator _FindUser(Keraysera era)
    {
        while (Settings.token == "")
            yield return null;
        var requestUri = uri + era.pokaName + "/" + id + "/time.json" + "?shallow=true" + "&auth=" + Settings.token;
        using (var www = UnityWebRequest.Get(requestUri))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.timeout = 5;
            var request = www.SendWebRequest();
            yield return request;
            if (www.isNetworkError || www.isHttpError)
            {
                if (Time.realtimeSinceStartup - tokenLastFetched > tokenFetchUpdateLimit)
                    StartCoroutine(GetAuth());
                era.onUserBestTimeUpdated(NetWorkResponse.NoConnection, -2f);
            }
            else
            {
                string result = www.downloadHandler.text;
                if (result == "null") era.onUserBestTimeUpdated(NetWorkResponse.NoData, -1f);
                else
                {
                    var _time = float.Parse(result, CultureInfo.InvariantCulture.NumberFormat);
                    era.userBestTime = _time;
                    era.onUserBestTimeUpdated(NetWorkResponse.Success, _time);
                }
            }
        }
    }
    private IEnumerator GetScores(Keraysera era)
    {
        while (Settings.token == "")
            yield return null;
        string orderBy = "\"time\"";
        using (var www = UnityWebRequest.Get(uri + era.pokaName + ".json" + "?orderBy=" + orderBy + "&limitToFirst=100" + "&auth=" + Settings.token))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.timeout = 5;
            var request = www.SendWebRequest();
            yield return request;
            if (www.isNetworkError || www.isHttpError)
            {
                if (Time.realtimeSinceStartup - tokenLastFetched > tokenFetchUpdateLimit)
                    StartCoroutine(GetAuth());
                era.onHighScoreListLoaded(NetWorkResponse.NoConnection);
            }
            else
            {
                string result = www.downloadHandler.text;
                Dictionary<string, HighScoreEntry> entryDict = JsonConvert.DeserializeObject<Dictionary<string, HighScoreEntry>>(result);
                if (entryDict != null)
                {
                    var temp = entryDict.OrderBy(a => a.Value.time);
                    era.userLeaderBoardIndex = temp.Select(x => x.Key).ToList().IndexOf(id);
                    era.highScoreList = temp.Select(x => x.Value).ToList();
                    era.isHighScoreListUpdated = true;
                    era.timeSinceLastListUpdate = Time.realtimeSinceStartup;
                    era.onHighScoreListLoaded(NetWorkResponse.Success);
                }
                else
                {
                    era.onHighScoreListLoaded(NetWorkResponse.NoData);
                }
            }
        }
    }

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        Instance.name = "HighScoreManager";
    }
}


public class IdToken
{
    public string idToken;
}
