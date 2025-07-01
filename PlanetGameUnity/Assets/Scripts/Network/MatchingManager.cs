using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MatchingManager : MonoBehaviour
{
    TMP_InputField inputField;
    const string MATCH_API_ENDPOINT = "/api/match";
    
    
    void Start()
    {
        inputField = GameObject.Find("Keyword").GetComponent<TMP_InputField>();
    }
    void Update()
    {
        
    }
    bool isWaiting;//マッチ待機中か
    const int CHARACTER_LIMIT = 10;//合言葉の文字数上限
    /// <summary>
    /// InputFieldから読み取った合言葉で部屋を探す
    /// </summary>
    public void SendKeyword()
    {
        if(inputField.text.Length == 0 && inputField.text.Length <= CHARACTER_LIMIT)
        {
            Debug.Log("入力エラー");
            return;
        }
        //マッチ待機中なら実行しない
        if (!isWaiting)
        {
            StartCoroutine(FindRoom(inputField.text, PlayerIdManager.Id, ApiConfig.BASE_URI + MATCH_API_ENDPOINT));
        }
    }
    const float REQUEST_INTERVAL = 1f;
    /// <summary>
    /// GetPlayerCountInRoom()を一定間隔で呼ぶ
    /// </summary>
    /// <returns></returns>
    IEnumerator GetPlayerCountInRoomLoop()
    {
        while (isWaiting) 
        {
            yield return StartCoroutine(GetPlayerCountInRoom(ApiConfig.BASE_URI + "/api/room/" + RoomId + "/playerCount"));
            Debug.Log("マッチ待機中");
            yield return new WaitForSeconds(REQUEST_INTERVAL);
        }
    }
    const int MAX_PLAYER_COUNT = 2;
    /// <summary>
    /// 参加した部屋の人数を取得し、2人だったらシーン遷移する
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    IEnumerator GetPlayerCountInRoom(string uri)
    {
        UnityWebRequest request = new UnityWebRequest(uri, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            PlayerCount res = JsonUtility.FromJson<PlayerCount>(json);
            Debug.Log(res.player_count);
            if(res.player_count == MAX_PLAYER_COUNT)
            {
                SceneChangeManager.SceneChange("RoleSetScene");
            }
        }
        else
        {
            Debug.Log("Error" + request.error);
        }
    }

    /// <summary>
    /// 入力された合言葉で部屋を探し、作成or参加する
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="playerId"></param>
    /// <param name="uri"></param>
    /// <returns></returns>
    IEnumerator FindRoom(string keyword, string playerId, string uri)
    {
        //リクエストのボディ
        string json = JsonUtility.ToJson(new MatchData(keyword, playerId));
        byte[] rawData = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(uri, "POST");
        request.uploadHandler = new UploadHandlerRaw(rawData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        isWaiting = true;
        //結果が返って来るまで待機
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            NetworkStateManager.SetState(NetworkStateManager.NetworkState.Connected);
            Debug.Log("Success" + request.downloadHandler.text);
            string res = request.downloadHandler.text;
            RoomData roomData = JsonUtility.FromJson<RoomData>(res);
            RoomId = roomData.room_id;
            IsHost = roomData.is_host;
            Debug.Log("RoomId : " + roomData.room_id + "isHost" + roomData.is_host);
            
            //部屋のプレイヤーの数を一定間隔で見に行く
            StartCoroutine(GetPlayerCountInRoomLoop());
        }
        else if (request.responseCode == 403)
        {
            isWaiting = false;
            Debug.Log("Full : "+request.downloadHandler.text);
        }
        else
        {
            isWaiting = false;
            Debug.Log("Error" + request.error);
        }
    }
    [System.Serializable]
    class MatchData
    {
        public string keyword;//マッチングに使う合言葉
        public string player_id;//自分のID

        public MatchData(string keyword, string player_id)
        {
            this.keyword = keyword;
            this.player_id = player_id;
        }
    }
    [System.Serializable]
    class RoomData
    {
        public int room_id;
        public bool is_host;
    }
    public static bool IsHost { get; private set; }
    public static bool IsCommander { get; set; }
    public static int RoomId { get; private set; }
    
    [System.Serializable]
    class PlayerCount
    {
        public int player_count;//自分の入っている部屋の人数
    }
}
