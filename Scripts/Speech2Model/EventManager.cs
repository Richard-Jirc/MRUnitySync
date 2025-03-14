using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static event Action<string> OnError;
    public static event Action<string> OnWarning;

    public static void HandleError(string message)
    {
        Debug.LogError($"Error: {message}");
        OnError?.Invoke(message);
    }

    public static void HandleWarning(string message)
    {
        Debug.LogWarning($"Warning: {message}");
        OnWarning?.Invoke(message);
    }


}
