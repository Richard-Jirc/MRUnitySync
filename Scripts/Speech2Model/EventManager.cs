using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static. Centric infomation and event handler.
/// </summary>
public static class EventManager
{
    public static bool showActionLog = true;
    public static event Action<string> OnError;
    public static event Action<string> OnWarning;

    public static void HandleError(string message)
    {
        Debug.LogError($"<App Error> {message}");
        OnError?.Invoke(message);
    }

    public static void HandleWarning(string message)
    {
        Debug.LogWarning($"<App Warn> {message}");
        OnWarning?.Invoke(message);
    }

    public static void ActionLog(string message)
    {
        if (!showActionLog) return;
        Debug.Log($"<App> {message}");
    }

}
