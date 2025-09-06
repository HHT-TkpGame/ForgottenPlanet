using System;
using System.Linq;
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
        poller.StartLoop(
            chatList =>
            {
                chatHistory = uiController.DisplayChat(chatList);
            });
    }

    public void OnSendChat()
    {
        string msg = uiController.GetInputText();
        if (string.IsNullOrEmpty(msg)) return;
        StartCoroutine(Client.SendChatMessage(
            msg,
            res => { },
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
    /// ÉVÅ[ÉìëJà⁄Ç≥ÇÍÇΩÇÁéQè∆ÇµÇ»Ç®Çµ
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChatUIController[] controllers = FindObjectsByType<ChatUIController>(FindObjectsSortMode.None);
        if (controllers.Length > 1)
        {
            foreach (var ctr in controllers)
            {
                if (ctr.IsCommander == MatchingManager.IsCommander)
                {
                    uiController = ctr;
                    break;
                }
            }
        }
        else
        {
            uiController = controllers[0];
        }
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