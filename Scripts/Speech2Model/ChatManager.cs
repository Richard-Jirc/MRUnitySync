using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


/* Agent Names as Keys:

- Reason
- Code
- Optimize

*/


/// <summary>
/// Singleton. <br/>
/// Use "Instance" to get methods.
/// </summary>
public class ChatManager : MonoBehaviour
{
    private Dictionary<string, JArray> _chatHistory;


    #region PUBLIC: CHAT

    /// <summary>
    /// Push a chat message to ReasonAgent.
    /// </summary>
    /// <param name="input">Chat message.</param>
    public async Task<bool> Chat_ReasonAgent(string input)
    {
        if (input.Length == 0)
        {
            EventManager.HandleWarning("ChatRA: input empty");
            return false;
        }

        p_UpdateChatHistory("Reason", input);

        JObject response = await LLMRequestHandler.Instance.AskLLM(_agentLLMChoice["Reason"], _chatHistory["Reason"]);

        if (response == null)
        {
            EventManager.HandleError("ChatRA: chat session failed.");
            return false;
        }

        string responseContent = p_ProcessGPTResponse(response);
        // Debug.Log(responseContent);

        p_UpdateChatHistory("Reason", responseContent, false);

        return true;
    }
    #endregion
    


    #region PUBLIC: GET
    public void PrintChatHistory(string agent)
    {
        if (!_chatHistory.ContainsKey(agent))
        {
            EventManager.HandleWarning("PrintChatHistory: Invalid agent name");
            return;
        }

        Debug.Log(_chatHistory[agent].ToString(Formatting.Indented));
    }
    #endregion



    #region LIFE CYCLE
    public static ChatManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        p_InitAllAgents();
    }
    #endregion



    #region PROMPTS

    /// <summary>
    /// LLM choice for each agent.
    /// </summary>
    private Dictionary<string, string> _agentLLMChoice = new Dictionary<string, string>{
        {"Reason", "gpt-4o"},
        {"Code", "gpt-4o"},
        {"Optimize", "gpt-4o"}
    };

    /// <summary>
    /// Dictionary to apply zero/few-shot prompt for LLM Agents.
    /// </summary>
    private Dictionary<string, string> _agentSystemPrompt = new Dictionary<string, string>{
        {"Reason",
         "You are an extremely polite Chinese person that understands English but replies in Chinese."},

        {"Code",
         "Hello"},

        {"Optimize",
         "Hello"}
    };

    #endregion



    #region PRIVATE METHODS

    /// <summary>
    /// Set up developer promopt and chat history dictionary upon Start().
    /// </summary>
    private void p_InitAllAgents()
    {
        _chatHistory = new Dictionary<string, JArray>();
        
        foreach (KeyValuePair<string, string> pair in _agentSystemPrompt)
        {
            if (!_chatHistory.ContainsKey(pair.Key))
            {
                _chatHistory[pair.Key] = new JArray {
                    new JObject {
                        ["role"] = "developer",
                        ["content"] = pair.Value
                    }
                }; // Initialize chat history with prompting texts.
                EventManager.ActionLog($"ChatHistory Created for {pair.Key}");
            }
        }
    }


    private void p_UpdateChatHistory(string agent, string input, bool isUser = true)
    {
        if (!_chatHistory.ContainsKey(agent) || input.Length == 0) // Check if input valid.
        {
            EventManager.HandleWarning($"UpdateChatHistory: Input string invalid: {agent} - {input}");
            return;
        }
        
        string role = isUser? "user" : "assistant";

        _chatHistory[agent].Add(new JObject {
            ["role"] = role,
            ["content"] = input
        });
    }


    /// <summary>
    /// Process GPT style JObject response to string content from LLM.
    /// </summary>
    /// <param name="input">JObject of full response body.</param>
    /// <returns>String of LLM response content.</returns>
    private string p_ProcessGPTResponse(JObject input)
    {
        if (input == null)
        {
            EventManager.HandleWarning("GPT Response Process: input null");
            return null;
        }

        return input["choices"]?[0]?["message"]?["content"]?.ToString();
    }

    #endregion
}
