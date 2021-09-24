using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerHelper
{
    public static void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        for (int i = 0; i < go.transform.childCount; i++)
        {
            SetLayerRecursively(go.transform.GetChild(i).gameObject, layer);
        }
    }
}
