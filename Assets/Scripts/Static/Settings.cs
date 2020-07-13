using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class Settings
{
    public static int shadows = PlayerPrefs.GetInt("shadows", 0);
    public static int sound = PlayerPrefs.GetInt("sound", 0);
    public static Keraysera[] kerayserat = Resources.LoadAll<Keraysera>("Kerayserat");
    public static Keraysera keraysera = kerayserat[0];
    public static Kerayskone[] kerayskoneet = Resources.LoadAll<Kerayskone>("Kerayskoneet");
    public static Kerayskone kerayskone = kerayskoneet[2];
    public static TextMeshProUGUI debugText;
    public static int resolution = PlayerPrefs.GetInt("resolution", 0);
    public static string username = PlayerPrefs.GetString("username", "");
    public static Color buttonPressed = new Color32(183, 119, 49, 183);
    // public static Vector2 nativeResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
    public static string token = "";

    public static void SaveSetting(string name, int value)
    {
        typeof(Settings).GetField(name).SetValue(null, value);
        PlayerPrefs.SetInt(name, value);
        if (name == "resolution")
        {
            SetResolution(value);
        }
    }
    public static void NameChange(string newName)
    {
        PlayerPrefs.SetString("username", newName);
        username = newName;
        Debug.Log(newName);
    }
    public static void ResetKeraysEraDelegates()
    {
        for (int i = 0; i < kerayserat.Length; i++)
        {
            kerayserat[i].ResetDelegates();
        }
    }
    public static void SetResolution(int index)
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
                break;
            case 1:
                Screen.SetResolution(1280, 720, FullScreenMode.ExclusiveFullScreen);
                break;
            default:
                Screen.SetResolution(960, 540, FullScreenMode.ExclusiveFullScreen);
                break;
        }
    }
}

