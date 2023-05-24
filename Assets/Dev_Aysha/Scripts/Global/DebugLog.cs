using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLog : MonoBehaviour
{
    public bool EnableLogs = true;
    public bool EnableLogErrors = true;
    public static bool enableLogs = false;
    public static bool enableLogErrors = false;


    private void Awake()
    {
        enableLogs = EnableLogs;
        enableLogErrors = EnableLogErrors;
    }
    public static void LogError(string msg)
    {
        if (!enableLogErrors)
            return;
        Debug.Log("<color=red>Error: </color> " + msg);
    }
    public static void Log(string msg)
    {
        if (!enableLogs)
            return;
        Debug.Log("<color=green>Log: </color> " + msg);
    }
}
