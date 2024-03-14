using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App
{
    public static void Trace(object t, DebugColor color = DebugColor.normal)
    {
        Debug.Log("<color=" + color.ToString() + ">" + t.ToString() + "</color>");
    }

    internal static void Trace(string v1, string v2)
    {
        throw new NotImplementedException();
    }

    public enum DebugColor
    {
        normal, white, yellow, red, green 
    }
}
