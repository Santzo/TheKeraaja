using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IUIHandler
{
    void OnDown(Transform trans);
    void OnUp(Transform trans);
}

public interface IUIObject: IPointerDownHandler, IPointerUpHandler
{
}
public interface IMainMenuHandler
{
    void OnDown(Transform trans);
    void OnUp(Transform trans);
}
public interface IDropDownHandler
{
    void OnDropDown(Transform trans, string[] options);
    void OnDropUp(Transform trans, string[] options);
}

