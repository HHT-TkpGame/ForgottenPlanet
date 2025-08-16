using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChatManager : MonoBehaviour
{
    [SerializeField] ChatUIController uiController;
    [SerializeField] ChatClientPoller poller;
    string chatHistory;
    public static ChatManager Instance { get; private set; }
    public ChatClient Client { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Client = new ChatClient();
    }

    void Start()
    {
        Debug.Log("Start");
        poller.StartLoop(
            chatList =>
            {
                chatHistory = uiController.DisplayChat(chatList);
            });
    }

    public void OnSendChat()
    {
        string msg = uiController.GetInputText();
        Debug.Log("���M���悤�Ƃ��Ă��郁�b�Z�[�W��" + msg);
        Debug.Log(string.IsNullOrEmpty(msg));
        if (string.IsNullOrEmpty(msg)) return;
        Debug.Log("���M�J�n");
        StartCoroutine(Client.SendChatMessage(
            msg,
            res => Debug.Log("���M����"),
            onError: () => { }
        ));
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    /// <summary>
    /// �V�[���J�ڂ��ꂽ��Q�Ƃ��Ȃ���
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        uiController = FindAnyObjectByType<ChatUIController>();
        poller = FindAnyObjectByType<ChatClientPoller>();
        if (uiController != null && poller != null)
        {
            uiController.RestoreChat(chatHistory, OnSendChat);
            poller.StartLoop(
            chatList =>
            {
                chatHistory = uiController.DisplayChat(chatList);
            });

        }

    }
}