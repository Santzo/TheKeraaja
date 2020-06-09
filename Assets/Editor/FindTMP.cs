using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using System;
using System.Security.AccessControl;

public class FindTMP: EditorWindow
{
    ObjectField ab, bb;
    [MenuItem("Helpers/FindTMP", false, 0)]
    public static void Create()
    {
        var window = GetWindow<FindTMP>();
        window.minSize = new Vector2(300, 100);
    }
    private void OnEnable()
    {
        ab = new ObjectField();
        ab.objectType = typeof(TMP_FontAsset);
        bb = new ObjectField();
        bb.objectType = typeof(Material);
        var but = new Button() { text = "Replace fonts"};
        rootVisualElement.Add(ab);
        rootVisualElement.Add(bb);
        rootVisualElement.Add(but);
        but.RegisterCallback<MouseUpEvent>(aa) ;
 
    }

    private void aa(MouseUpEvent evt)
    {
        var objs = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].font = ab.value as TMP_FontAsset;
            objs[i].fontSharedMaterial = bb.value as Material;
        }
    
    }
    private class TextAss
    {
        public TMP_FontAsset asset;
        public Material mat;
    }
}
