using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HeartbeatManager : MonoBehaviour
{
    const string BASE_URI = "https://hht-game.fee-on.com/SynchronizationTest";
    const string API_ENDPOINT = "/api/player/heartbeat";
    const float REQUEST_INTERVAL = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SendHeartbeatLoop(BASE_URI + API_ENDPOINT));
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator SendHeartbeatLoop(string uri)
    {
        while (true)
        {
            yield return StartCoroutine(SendHeartbeat(uri));

            yield return new WaitForSeconds(REQUEST_INTERVAL);
        }
    }
    IEnumerator SendHeartbeat(string uri)
    {
        UnityWebRequest request = new UnityWebRequest(uri, "POST");
        string json = JsonUtility.ToJson(new RoomAndPlayerId(MatchingManager.RoomId, PlayerIdManager.Id));
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(rawData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.Success)
        {
            NetworkStateManager.SetState(NetworkStateManager.NetworkState.Connected);
            Debug.Log(request.downloadHandler.text);
        }
        else if (request.responseCode == 404)
        {
            Debug.Log("í êMÇ™êÿífÇ≥ÇÍÇ‹ÇµÇΩ");
            NetworkStateManager.SetState(NetworkStateManager.NetworkState.Disconnected);
        }
        else
        {
            Debug.Log(request.error);
        }
    }
    class RoomAndPlayerId
    {
        public int room_id;
        public string player_id;
        public RoomAndPlayerId(int roomId, string playerId)
        {
            room_id = roomId;
            player_id = playerId;
        }
    }
}
