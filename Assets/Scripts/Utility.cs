using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static void PrintList<T>(List<T> list)
    {
        string text = "{ " + string.Join(", ", list) + " }";
        Debug.Log(text);
    }

    public static void PrintArray<T>(T[] array)
    {
        string text = "{ " + string.Join(", ", array) + " }";
        Debug.Log(text);
    }

    public static void PrintDictionary<T1, T2>(Dictionary<T1, T2> dic)
    {
        string text = "{ " + string.Join(", ", dic) + " }";
        Debug.Log(text);
    }

    public static void PrintQueue<T>(Queue<T> queue)
    {
        string text = "{ " + string.Join(", ", queue) + " }";
        Debug.Log(text);
    }

    public static void PrintStack<T>(Stack<T> stack)
    {
        string text = "{ " + string.Join(", ", stack) + " }";
        Debug.Log(text);
    }
}
