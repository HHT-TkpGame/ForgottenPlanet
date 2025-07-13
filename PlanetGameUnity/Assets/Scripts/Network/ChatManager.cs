using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ChatManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text txtMsg;
    const float REQUEST_INTERVAL = 4f;

    DateTime lastFetchTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FetchChatLoop($"{ApiConfig.BASE_URI}/api/room/{MatchingManager.RoomId}/chat/"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// サーバーにチャットを送るコルーチンを動かすためのメソッド
    /// </summary>
    public void SendChat()
    {
        if (inputField.text.Length != 0)
        {
            StartCoroutine(SendChatMessage($"{ApiConfig.BASE_URI}/api/room/{MatchingManager.RoomId}/chat/", inputField.text));
            inputField.text = "";
        }
    }
    /// <summary>
    /// 取得したチャットをUI上のチャットログに反映
    /// </summary>
    void DisplayChat(ChatDataList chatDataList)
    {
        string sender;
        for(int i = 0; i < chatDataList.messages.Length; i++)
        {
            if (chatDataList.messages[i].player_id == PlayerIdManager.Id)
            {
                sender = "you";
            }
            else
            {
                sender = "partner";
            }
            txtMsg.text += $"{sender} : {chatDataList.messages[i].message}\n";
            
        }
    }
    /// <summary>
    /// サーバーにチャットを送る
    /// </summary>
    /// <param name="uri">APIまでのURL</param>
    /// <param name="message">チャットの内容</param>
    /// <returns></returns>
    IEnumerator SendChatMessage(string uri, string message)
    {
        UnityWebRequest request = new UnityWebRequest(uri, "POST");
        string json = JsonUtility.ToJson(new ChatData(PlayerIdManager.Id, message));
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(rawData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            StartCoroutine(FetchChats(uri));
        }
        else
        {
            Debug.Log(request.error);
        }
    }
    //ポーリングで定期的にチャットを取得する
    IEnumerator FetchChatLoop(string uri)
    {
        while(NetworkStateManager.CurrentState == NetworkStateManager.NetworkState.Connected)
        {
            yield return StartCoroutine(FetchChats(uri));
            yield return new WaitForSeconds(REQUEST_INTERVAL);
        }
    }
    //チャットを取得する
    IEnumerator FetchChats(string uri)
    {
        //ToString("o")でサーバー側のdate型バリデーションが通るようにする
        string since = lastFetchTime.ToString("o");
        string reqUri = $"{uri}?since={UnityWebRequest.EscapeURL(since)}&player_id={UnityWebRequest.EscapeURL(PlayerIdManager.Id)}";
        UnityWebRequest request = new UnityWebRequest(reqUri, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string res = request.downloadHandler.text;
            ChatDataList chats = JsonUtility.FromJson<ChatDataList>(res);
            //テスト表示
            foreach(ChatData msg in chats.messages)
            {
                Debug.Log($"{msg.player_id}:{msg.message} at {msg.sent_at}");
            }
            //chats.messages.lengthが0だったら(チャットがなかったら)通らない
            if (chats.messages.Length > 0)
            {
                //DateTime型に変換できたらmessagesの末尾のsent_atを最新更新時間とする
                if (DateTime.TryParse(chats.messages[chats.messages.Length - 1].sent_at, out DateTime time))
                {
                    lastFetchTime = time;
                }
            }
            //ログに表示
            DisplayChat(chats);
        }
        else
        {
            Debug.Log(request.error);
        }
    }
    //ChatDataをラップするためのクラス
    [System.Serializable]
    class ChatDataList
    {
        public ChatData[] messages;
    }
    [System.Serializable]
    class ChatData
    {
        public string player_id;
        public string message;
        public string sent_at;
        public ChatData(string playerId, string message)
        {
            this.player_id = playerId;
            this.message = message;
            this.sent_at = "";
        }
    }
}
