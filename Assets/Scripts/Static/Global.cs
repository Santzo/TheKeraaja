using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static string FromFloatToTime(float time, int decimals = 3)
    {
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.Floor(time - minutes * 60);
        string dec = "1".PadRight(decimals + 1, '0');
        float milliseconds = Mathf.Floor((time % 1) * int.Parse(dec));
        return string.Format("{0:00}:{1:00},{2:0}", minutes, seconds, milliseconds);
    }
}
