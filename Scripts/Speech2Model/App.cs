using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// App Session Manager Class.
/// </summary>
public class App : MonoBehaviour
{


    #region LIFE CYCLE

    void Start()
    {
        TestChat();
    }
    void Oestroy() {}

    #endregion



    private int _chatLimit = 5;
    private string[] testingResponses = new string[]
    {
        "Tell me who you are",
        "How are you doing?",
        "I don't feel so good.",
        "Thank you for your service"
    };
    private async void TestChat()
    {
        await Task.Delay(2000);

        int i = 0;
        while (i < _chatLimit)
        {
            await Task.Delay(2000);

            string answer = testingResponses[Random.Range(0, testingResponses.Length)];

            bool result = await ChatManager.Instance.Chat_ReasonAgent(answer);
            if (!result)
            {
                EventManager.HandleError($"Chat Failed with {i}th attempt");
                break;
            }

            EventManager.ActionLog($"Conversation {i} success");
            i++;
        }

        ChatManager.Instance.PrintChatHistory("Reason");
    }
}
