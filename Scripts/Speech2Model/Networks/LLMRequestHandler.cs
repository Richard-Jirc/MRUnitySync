using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Singleton.
/// </summary>
public class LLMRequestHandler : MonoBehaviour
{
    [SerializeField] private const string GPT_Url = "https://api.openai.com/v1/chat/completions";
    [SerializeField] private string GPT_KeyPath;
    private string GPT_Key;


    #region PUBLIC METHODS
    public void AskGPT(string model, JArray chatArray)
    {
        StartCoroutine(GPTRequest(model, chatArray));
    }

    #endregion




    private IEnumerator GPTRequest(string model, JArray chatArray)
    {
        string jsonData = new JObject
        {
            ["model"] = model,
            ["messages"] = chatArray
        }.ToString();

        byte[] postData = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(GPT_Url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + GPT_Key);

            yield return request.SendWebRequest();
            
            JObject response = JObject.Parse(request.downloadHandler.text);
            Debug.Log(response.ToString());
        }
    }


    



    /// <summary>
    /// Load secret key from local project file.
    /// </summary>
    private void LoadGPTApiKey()
    {
        string path = Application.dataPath + GPT_KeyPath;

        if (File.Exists(path))
        {
            GPT_Key = File.ReadAllText(path).Trim(); // Read the key and remove any extra spaces
        }
        else
        {
            Debug.LogError("API Key file not found at: " + path);
        }
    }



    #region LIFE CYCLE
    public static LLMRequestHandler Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        LoadGPTApiKey();
    }

    void Start()
    {
        JArray chatHistory = new JArray
        {
            new JObject {
                ["role"] = "developer",
                ["content"] = "You are a very rude person and do not lose any quarrels!"
            },
            new JObject {
                ["role"] = "user",
                ["content"] = "What is wrong with you?"
            },
        };
        AskGPT("gpt-4o", chatHistory);
    }

    #endregion
}



/* Response Example
{
    "id": "chatcmpl-BAHMcSvzIPmOxCg8zyR3XP46P77an",
    "object": "chat.completion",
    "created": 1741789726,
    "model": "gpt-4o-2024-08-06",
    "choices": [
        {
        "index": 0,
        "message": {
            "role": "assistant",
            "content": "Why did the AI bring a ladder to the computer?  \n  \nIt wanted to reach the high-speed internet!",
            "refusal": null,
            "annotations": []
        },
        "logprobs": null,
        "finish_reason": "stop"
        }
    ],
    "usage": {
        "prompt_tokens": 22,
        "completion_tokens": 22,
        "total_tokens": 44,
        "prompt_tokens_details": {
        "cached_tokens": 0,
        "audio_tokens": 0
        },
        "completion_tokens_details": {
        "reasoning_tokens": 0,
        "audio_tokens": 0,
        "accepted_prediction_tokens": 0,
        "rejected_prediction_tokens": 0
        }
    },
    "service_tier": "default",
    "system_fingerprint": "fp_f9f4fb6dbf"
}
*/
