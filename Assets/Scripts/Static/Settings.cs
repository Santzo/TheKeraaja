using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class Settings
{
    public static int shadows = PlayerPrefs.GetInt("shadows", 0);
    public static int sound = PlayerPrefs.GetInt("sound", 0);
    public static Keraysera[] kerayserat = Resources.LoadAll<Keraysera>("Kerayserat");
    public static Keraysera keraysera;
    public static Kerayskone[] kerayskoneet = Resources.LoadAll<Kerayskone>("Kerayskoneet");
    public static Kerayskone kerayskone = kerayskoneet[2];
    public static TextMeshProUGUI debugText;
    public static int resolution = PlayerPrefs.GetInt("resolution", 0);
    public static string username = PlayerPrefs.GetString("username", "");
    public static Color buttonPressed = new Color32(183, 119, 49, 183);

    public static void SaveSetting(string name, int value)
    {
        typeof(Settings).GetField(name).SetValue(null, value);
        PlayerPrefs.SetInt(name, value);
        Debug.Log(sound);
    }
    public static void NameChange(string newName)
    {
        PlayerPrefs.SetString("username", newName);
        username = newName;
        Debug.Log(newName);
    }
}

