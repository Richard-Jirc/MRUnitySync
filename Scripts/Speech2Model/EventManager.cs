using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static event Action<string> OnError;

    public static void HandleError(string message)
    {
        Debug.LogError($"Error: {message}");
        OnError?.Invoke(message);
    }

}
