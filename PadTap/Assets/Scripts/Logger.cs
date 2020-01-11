using System;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static string NoComponentFound(Type ofType)
    {
        return "No object with component " + ofType + " found!";
    }

    public static string NotAssigned(Type component, Type toScript, string inObject)
    {
        return "No object with component" + component + " assigned to " + toScript + " in " + inObject;
    }

    public static string ReceivedNull(Type parameter)
    {
        return parameter + " received is null";
    }
}
