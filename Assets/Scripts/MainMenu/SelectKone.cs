using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectKone : MonoBehaviour, IUIObject
{
    IMainMenuHandler menuHandler;
    Image nopeus, kiihtyvyys, kaantyvyys, jarrutus, background;
    Vector3 konePos = new Vector3(260f, -154f, -258f);
    Vector3 oriRot = new Vector3(-1.25f, 147f, 0f);
    Animator anim;
    int selectedHash;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        background = GetComponent<Image>();
        nopeus = transform.Find("NopeusFront").GetComponent<Image>();
        kiihtyvyys = transform.Find("KiihtyvyysFront").GetComponent<Image>();
        kaantyvyys = transform.Find("KaantyvyysFront").GetComponent<Image>();
        jarrutus = transform.Find("JarrutusFront").GetComponent<Image>();
        menuHandler = GetComponentInParent<IMainMenuHandler>();
        selectedHash = Animator.StringToHash("Selected");
    }
    public void SetValues(Kerayskone kone)
    {
        nopeus.fillAmount = kone.nopeus * 0.1f;
        kiihtyvyys.fillAmount = kone.kiihtyvyys * 0.1f;
        kaantyvyys.fillAmount = kone.kaantyvyys * 0.1f;
        jarrutus.fillAmount = kone.jarrutus * 0.1f;
        var obj = Instantiate(kone.keraysKone, transform, true);
        obj.name = "Kerayskone";
        //obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = konePos;
        anim.Rebind();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        menuHandler.OnUp(transform);
    }

    internal void UpdateSelection(bool v)
    {
        background.color = v ? Settings.buttonPressed : Color.clear;
        anim.SetBool(selectedHash, v);
    }
}
