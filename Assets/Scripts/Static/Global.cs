using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static string FromFloatToTime(float time, int decimals = 3)
    {
        return $"{(int)time / 60:0}:{time % 60:00.000}";
    }
}
