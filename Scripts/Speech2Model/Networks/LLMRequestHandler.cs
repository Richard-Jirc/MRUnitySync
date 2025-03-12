using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
using System.IO;

public class LLMRequestHandler : MonoBehaviour
{
    [SerializeField] private const string GPT_Url = "https://api.openai.com/v1/chat/completions";
    [SerializeField] private string GPT_KeyPath;
    private string GPT_Key;

    public void SendLLMRequest(string prompt)
    {
        StartCoroutine(GPTRequest(prompt));
    }


    private IEnumerator GPTRequest(string prompt)
    {
        string jsonData = _packBody_GPT("gpt-4o", prompt);

        byte[] postData = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(GPT_Url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + GPT_Key);

            yield return request.SendWebRequest();
            
            Debug.Log(request.downloadHandler.text);
        }
    }



    #region GPT ReqBody Process
    private string _packBody_GPT(string model, string userPrompt, string developerPrompt = "You are an assistant.")
    {
        var requestBody = new Dictionary<string, object>
        {
            { "model", model },
            { "messages", new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string> { { "role", "developer" }, { "content", developerPrompt } },
                    new Dictionary<string, string> { { "role", "user" }, { "content", userPrompt } }
                }
            }
        };
        return JsonConvert.SerializeObject(requestBody, Formatting.None);
    }
    #endregion



    private void LoadApiKey()
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
    
    void Awake()
    {
        LoadApiKey();
    }

    void Start()
    {
        SendLLMRequest("Say a joke of yourself.");
    }

    #endregion
}
