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

    public static KeraysLista currentRivi = null;
    public static Action<Vector2> applyForce = delegate { };
    public static Action onStartLoading = delegate { };
    public static Action<float> loadProgress = delegate { };
    public static Action<bool> onStartCollecting = delegate { };
    public static bool pokaValmis = false;
    public static Action<bool> onCloseToCollectionPoint = delegate { };
    public static Action<int> onUpdateRemainingAmount = delegate { };
    public static Action seuraavaRivi = delegate { };
    public static Action<float> onTapMovementSpeed = delegate { };
    public static Action boxCollected = delegate { };
    public static Transform currentRullakko;
}
