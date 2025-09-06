using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChatUIController : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text txtMsg;
    [SerializeField] Button sendButton;
    RectTransform scrollViewRect;
    float scrollViewDefaultHeight;
    Vector2 scrollViewDefaultPos;
    RectTransform chatPanelRect;
    float panelDefaultHeight;
    bool isOpen = true;
    [SerializeField] bool isScalable;
    [SerializeField] bool isCommander;//commanderはtrue,agentはfalse
    public bool IsCommander => isCommander;
    void Awake()
    {
        chatPanelRect = GetComponent<RectTransform>();
        scrollViewRect = transform.Find("Scroll View").GetComponent<RectTransform>();
        panelDefaultHeight = chatPanelRect.sizeDelta.y;
        scrollViewDefaultHeight = scrollViewRect.sizeDelta.y;
        scrollViewDefaultPos = scrollViewRect.anchoredPosition;
    }
    void Start()
    {
        if (isScalable || !MatchingManager.IsCommander)
        {
            ToggleChatPanel();
        }
    }
    public void ToggleChatPanel()
    {
        if (isOpen)
        {
            scrollViewRect.sizeDelta = new Vector2(scrollViewRect.sizeDelta.x, 130f);
            scrollViewRect.anchoredPosition = new Vector2(scrollViewRect.anchoredPosition.x, -45f);
            chatPanelRect.sizeDelta = new Vector2(chatPanelRect.sizeDelta.x, 180f);
            isOpen = false;
        }
        else
        {
            scrollViewRect.sizeDelta = new Vector2(scrollViewRect.sizeDelta.x, scrollViewDefaultHeight);
            scrollViewRect.anchoredPosition = scrollViewDefaultPos;
            chatPanelRect.sizeDelta = new Vector2(chatPanelRect.sizeDelta.x, panelDefaultHeight);
            isOpen = true;
        }
    }

    public void RestoreChat(string history, UnityAction onSend)
    {
        sendButton.onClick.RemoveAllListeners();
        sendButton.onClick.AddListener(onSend);
        txtMsg.text = history;
    }

    public string GetInputText()
    {
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
