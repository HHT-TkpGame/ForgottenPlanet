using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChatUIController : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text txtMsg;
    [SerializeField] Button sendButton;

    public void RestoreChat(string history, UnityAction onSend)
    {
        sendButton.onClick.RemoveAllListeners();
        sendButton.onClick.AddListener(onSend);
        txtMsg.text = history;
    }

    public string GetInputText()
    {
        Debug.Log("textClear");
        string text = inputField.text;
        inputField.text = "";
        return text;
    }
    /// <summary>
    /// 取得したチャットをUI上のチャットログに反映
    /// </summary>
    public string DisplayChat(ChatDataList chatDataList)
    {
        string sender;
        foreach (var message in chatDataList.messages)
        {
            sender = message.player_id == PlayerIdManager.Id ? "you" : "partner";
            txtMsg.text += $"{sender} : {message.message}\n";
        }
        return txtMsg.text;
    }
}
