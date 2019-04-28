using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions {
    public static void Shuffle<T>(this IList<T> list) {
        for (var i = 0; i < list.Count; i++)
            list.Swap(i, Random.Range(i, list.Count));
    }

    public static void Swap<T>(this IList<T> list, int i, int j) {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
}