using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public static bool isPlayerTurning = false;
    public static bool isPlayerCollecting = false;
    public static bool isPlayerCloseToCollectionPoint = false;
    public static bool isPlayerPickingUp = false;

    public static Action<Vector2> applyForce = delegate { };
    public static Action onStartLoading = delegate { };
    public static Action<float> loadProgress = delegate { };
    public static Action onStartCollecting = delegate { };
    public static Action<bool> onCloseToCollectionPoint = delegate { };
}
