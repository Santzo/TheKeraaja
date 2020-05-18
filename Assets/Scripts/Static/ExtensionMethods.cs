using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;


public static class ExtensionMethods
{
    public static void Populate<T>(this T[] arr) where T : new()
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = new T();
        }
    }

    public static SpriteRenderer AddRenderer(this GameObject ori, Material mat)
    {
        var sr = ori.AddComponent<SpriteRenderer>();
        sr.material = mat;
        return sr;
    }

    public static void EmptyTextArray(this TextMeshProUGUI[] ori)
    {
        for (int i = 0; i < ori.Length; i++)
        {
            ori[i].text = "";
        }
    }

    public static T[] LoadAssets<T>(this T[] arr, string path) where T : Object
    {
        var assetBundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.streamingAssetsPath, path));
        var assets = assetBundle.LoadAllAssets<T>();
        return assets;
    }

    public static Transform GetFromAllChildren(this Transform ori, string tag)
    {
        Transform result = null;
        foreach (Transform child in ori)
        {
            if (child.name == tag)
                return child;

            result = child.GetFromAllChildren(tag);

            if (result != null)
                return result;
        }

        return result;
    }
    public static void AddToList(this Transform ori, List<Transform> list, string tag)
    {
        foreach (Transform child in ori)
        {
            if (child.name.StartsWith(tag)) list.Add(child);
            child.AddToList(list, tag);
        }
    }
    public static Transform[] GetAllChildren(this Transform ori, string tag)
    {
        List<Transform> result = new List<Transform>();
        foreach (Transform child in ori)
        {
            child.AddToList(result, tag);
        }

        return result.ToArray();

    }
}
