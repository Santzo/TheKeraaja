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
    private static HighScoreManager _instance;
    public float lastRefresh = 0f;
    const string uri = "https://thekeraaja.firebaseio.com/";
    public Action<bool> OnScoresLoaded = delegate { };
    public List<HighScoreEntry> currentHighscores = new List<HighScoreEntry>();

    public void Start()
    {
        StartCoroutine(GetAuth());
    }
    public bool ScoresLoaded
    {
        get
        {
            return _scoresLoaded;
        }
        set
        {
            _scoresLoaded = value;
            OnScoresLoaded(_scoresLoaded);
        }
    }

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

    public void GetHighScores(string poka)
    {
        float timeSince = Time.time - Instance.lastRefresh;
        if (Instance.currentHighscores.Count == 0 || timeSince > 30f)
        {
            Instance.lastRefresh = Time.time;
            Instance.StartCoroutine(Instance.GetScores(poka));
        }
        else
        {
            Instance.ScoresLoaded = true;
        }
    }
    public void PostNewScore(HighScoreEntry score, Keraysera era)
    {
        StartCoroutine(PostScore(score, era));
    }
    public void FindUser(Keraysera era)
    {
        StartCoroutine(_FindUser(era));
    }
    private IEnumerator PostScore(HighScoreEntry score, Keraysera era)
    {
        while (Settings.token == "")
            yield return null;
        var id = SystemInfo.deviceUniqueIdentifier;
        var newScore = JsonUtility.ToJson(score);
        using (var www = UnityWebRequest.Put(uri + era.pokaName + "/" + id + ".json?auth=" + Settings.token, newScore))
        {
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(newScore));
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                era.onNewTimePosted("Ei internet-yhteyttä. Aikaasi ei päivitetty online-tietokantaan.");
            }
            else
            {
                era.onNewTimePosted("Aikasi on päivitetty online-tietokantaan.");
                era.userBestTime = score.time;
            }
        }
    }

    private IEnumerator GetAuth()
    {
        var userData = "{'returnSecureToken': 'true'}";
        // string api = "";
        var apiKey = Resources.Load<TextAsset>("Keraysera")?.text;
        var www = UnityWebRequest.Post("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + apiKey, userData);
        www.SetRequestHeader("Content-Type", "application/json");
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(userData));
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("aa" + www.error);
            Debug.Log(www.downloadHandler.text);
            yield return false;
        }
        else
        {
            string result = www.downloadHandler.text;
            var aa = JsonUtility.FromJson<IdToken>(result);
            Settings.token = aa.idToken;
            yield return true;
        }
    }
    private IEnumerator _FindUser(Keraysera era)
    {
        while (Settings.token == "")
            yield return null;
        var id = SystemInfo.deviceUniqueIdentifier;
        var requestUri = uri + era.pokaName + "/" + id + "/time.json" + "?shallow=true" + "&auth=" + Settings.token;
        Debug.Log(requestUri);
        using (var www = UnityWebRequest.Get(requestUri))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            var request = www.SendWebRequest();
            yield return request;
            if (www.isNetworkError || www.isHttpError)
            {
                // Debug.Log(www.error);
                // Debug.Log(www.downloadHandler.text);
                era.userBestTime = -2f;
            }
            else
            {
                string result = www.downloadHandler.text;
                Debug.Log(result);
                if (result == "null") era.userBestTime = 0f;
                else era.userBestTime = float.Parse(result, CultureInfo.InvariantCulture.NumberFormat);
                // var useri = JsonUtility.FromJson<HighScoreEntry>(result);
                // Debug.Log(useri.id == null);
            }
        }
    }
    private IEnumerator GetScores(string poka)
    {
        while (Settings.token == "")
            yield return null;
        string orderBy = "\"time\"";
        Debug.Log(orderBy);
        using (var www = UnityWebRequest.Get(uri + poka + ".json" + "?orderBy=" + orderBy + "&limitToLast=3" + "&auth=" + Settings.token))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            var request = www.SendWebRequest();
            yield return request;
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
                ScoresLoaded = false;
            }
            else
            {
                string result = www.downloadHandler.text;
                Dictionary<string, HighScoreEntry> entryDict = JsonConvert.DeserializeObject<Dictionary<string, HighScoreEntry>>(result);
                currentHighscores = entryDict.Select(x => x.Value).ToList();
                ScoresLoaded = true;
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
