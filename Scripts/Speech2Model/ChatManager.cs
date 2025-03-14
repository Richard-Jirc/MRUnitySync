using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json.Linq;


/* Agent Names as Keys:

- Reason
- Code
- Optimize

*/

public class ChatManager : MonoBehaviour
{
    private Dictionary<string, JArray> _chatHistory;


    #region PUBLIC METHODS
    public async void Chat_ReasonAgent(string input)
    {

    }

    #endregion



    #region LIFE CYCLE
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
         "Hello"},

        {"Code",
         "Hello"},

        {"Optimize",
         "Hello"}
    };

    #endregion



    #region PRIVATE METHODS


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
            }
        }
    }


    private void p_UpdateChatHistory(string agent, string input)
    {
        if (!_chatHistory.ContainsKey(agent) || input.Length == 0) // Check if input valid.
        {
            EventManager.HandleWarning($"UpdateChatHistory: Input string invalid: {agent} - {input}");
            return;
        }

        _chatHistory[agent].Add(new JObject {
            ["role"] = "user",
            ["content"] = input
        });
    }

    #endregion
}
