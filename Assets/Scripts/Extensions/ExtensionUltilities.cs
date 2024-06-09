using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionUltilities
{
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = 1; i < list.Count; i++)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
