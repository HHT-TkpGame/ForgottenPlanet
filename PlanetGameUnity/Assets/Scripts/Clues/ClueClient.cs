using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ClueClient : IClueSync
{
    public IEnumerator GetClue(Action<ClueSharedInfoList> onSuccess, Action<string> onError)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/clueShared/";
        UnityWebRequest request = new UnityWebRequest(uri, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            ClueSharedInfoList res = JsonUtility.FromJson<ClueSharedInfoList>(request.downloadHandler.text);
            onSuccess.Invoke(res);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }
    public IEnumerator PostClue(int clueId, Action onSuccess, Action<string> onError)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/clueId/" + clueId + "/clueShared/";
        UnityWebRequest request = new UnityWebRequest(uri, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.Success)
        {
            onSuccess.Invoke();
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }
    /// <summary>
    /// マッチ開始時に今回の真相IDと手がかりの範囲を取得する
    /// </summary>
    /// <param name="onSuccess"></param>
    /// <param name="onError"></param>
    /// <returns></returns>
    public IEnumerator GetClueAndTruth(Action<ServerCurrentMatchClues> onSuccess, Action<string> onError)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/clueAndTruth";
        UnityWebRequest request = new UnityWebRequest(uri, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            ServerCurrentMatchClues json = JsonUtility.FromJson<ServerCurrentMatchClues>(request.downloadHandler.text);
            onSuccess?.Invoke(json);
        }
        else
        {
            onError?.Invoke(request.error);
            Debug.Log(request.error);
        }
    }
    
}
