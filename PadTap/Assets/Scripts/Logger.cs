using System;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static void NoComponentFound(Type ofType)
    {
        Debug.LogError("No object with component " + ofType + " found!");
    }

    public static void NotAssigned(Type component, Type toScript, string inObject)
    {
        Debug.LogError("No object with component" + component + " assigned to " + toScript + " in " + inObject);
    }

    public static void ReceivedNull(Type parameter)
    {
        Debug.Log(parameter + " received is null");
    }

    public static void Error(string message)
    {
        Debug.LogError(message);
    }
}
