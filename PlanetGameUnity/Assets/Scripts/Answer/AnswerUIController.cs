using System;
using TMPro;
using UnityEngine;

public class AnswerUIController : MonoBehaviour
{
    [SerializeField] GameObject sendButton;
    [SerializeField] GameObject commanderAttention;
    [SerializeField] GameObject agentAttention;
    [SerializeField] GameObject infoText;
    [SerializeField] TMP_Text answerText;
    [SerializeField] TMP_Text[] truthList;
    int selectCount;
    int currentSelected;
    public event Action<int> OnSendButtonClicked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init(MatchingManager.IsCommander);
    }
    void Init(bool isCommander)
    {
        sendButton.SetActive(false);
        infoText.SetActive(false);
        answerText.gameObject.SetActive(false);
        if (isCommander)
        {
            commanderAttention.SetActive(true);
            agentAttention.SetActive(false);
        }
        else
        {
            commanderAttention.SetActive(false);
            agentAttention.SetActive(true);
        }
        currentSelected = 1;
    }
    public void OnSelected(int buttonId)
    {
        if (!MatchingManager.IsCommander) { return; }
        if (selectCount == 0)
        {
            commanderAttention.SetActive(false);
            sendButton.SetActive(true);
            infoText.SetActive(true);
            answerText.gameObject.SetActive(true);
        }
        answerText.text = truthList[buttonId].text;
        selectCount++;
        currentSelected = buttonId + 1;//ê^ëäIDÇ∆çáÇÌÇπÇÈÇΩÇﬂÇ…+1
    }
    public void SendAnswer()
    {
        AnswerSaver.answer_id = currentSelected;
        OnSendButtonClicked?.Invoke(currentSelected);
    }
}
