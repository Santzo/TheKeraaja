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


    public Action<bool> OnScoresLoaded = delegate { };
    public List<HighScoreEntry> currentHighscores = new List<HighScoreEntry>();


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

    public static void GetHighScores()
    {
        float timeSince = Time.time - Instance.lastRefresh;
        if (Instance.currentHighscores == null || timeSince > 30f)
        {
            Instance.lastRefresh = Time.time;
            Instance.StartCoroutine(Instance.GetScores());
        }
        else
        {
            Instance.ScoresLoaded = true;
        }
    }

    private IEnumerator PostScore(string newScore)
    {
        var www = UnityWebRequest.Post($"https://testi-f17c1.firebaseio.com/.json?", newScore);
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(newScore));
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload successful");
        }
    }

    private IEnumerator GetAuth()
    {
        var userData = "{'returnSecureToken': 'true'}";
        string api = ""; 
        api = Resources.Load<TextAsset>("info")?.text;
        var www = UnityWebRequest.Post("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + api, userData);
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(userData));
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("aa" + www.error);
            yield return false;
        }
        else
        {
            string result = www.downloadHandler.text;
            var aa = JsonUtility.FromJson<IdToken>(result);
            //StartCoroutine(GetScores(aa.idToken));
            yield return true;
        }
    }

    private IEnumerator GetScores()
    {
        var www = UnityWebRequest.Get($"https://testi-f17c1.firebaseio.com/.json");
        www.SetRequestHeader("Content-Type", "application/json");
        var request = www.SendWebRequest();
        yield return request;
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            ScoresLoaded = false;
            yield return false;
        }
        else
        {
            string result = www.downloadHandler.text;
            Dictionary<string, HighScoreEntry> entryDict = JsonConvert.DeserializeObject<Dictionary<string, HighScoreEntry>>(result);
            currentHighscores = entryDict.Select(x => x.Value).OrderByDescending(x => x.time).ToList();
          
            ScoresLoaded = true;
            yield return true;
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
