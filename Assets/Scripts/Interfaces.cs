using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IUIHandler
{
    void OnEnter(Transform trans);
    void OnExit(Transform trans);
    void OnDown(Transform trans);
    void OnUp(Transform trans);
}

public interface IUIObject: IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{
}
public interface IMainMenuHandler
{
    void OnEnter(Transform trans);
    void OnExit(Transform trans);
    void OnDown(Transform trans);
    void OnUp(Transform trans);
}

