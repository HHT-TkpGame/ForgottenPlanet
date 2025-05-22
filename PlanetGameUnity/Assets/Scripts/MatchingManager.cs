using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MatchingManager : MonoBehaviour
{
    const string BASE_URI = "https://hht-game.fee-on.com/SynchronizationTest";
    const string MATCH_API_ENDPOINT = "/api/match";
    
    
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    bool isWaiting;
    public void SendKeyword(string keyword)
    {
        if (!isWaiting)
        {
            StartCoroutine(FindRoom(keyword, PlayerIdManager.Id/*"takape"*/, BASE_URI + MATCH_API_ENDPOINT));
        }
    }
    const float REQUEST_INTERVAL = 1f;
    /// <summary>
    /// GetPlayerCountInRoom()を一定間隔で呼ぶ
    /// </summary>
    /// <returns></returns>
    IEnumerator GetPlayerCountInRoomLoop()
    {
        while (true) 
        {
            yield return StartCoroutine(GetPlayerCountInRoom(BASE_URI + "/api/room/" + RoomId + "/playerCount"));

            yield return new WaitForSeconds(REQUEST_INTERVAL);
        }
    }
    int playerCount;
    /// <summary>
    /// 参加した部屋の人数を取得する
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
            Debug.Log(request.downloadHandler.text);
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
            Debug.Log("Success" + request.downloadHandler.text);
            string res = request.downloadHandler.text;
            RoomData roomData = JsonUtility.FromJson<RoomData>(res);
            SaveRoomId(roomData.room_id);
            Debug.Log("RoomId : " + roomData.room_id + "/is_commander : " + roomData.is_commander);
            
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
        public string keyword;
        public string player_id;

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
        public bool is_commander;
    }
    public static int RoomId { get; private set; }
    void SaveRoomId(int roomId)
    {
        RoomId = roomId;
    }
    [System.Serializable]
    class PlayerCount
    {
        public int playerCount;
    }
}
