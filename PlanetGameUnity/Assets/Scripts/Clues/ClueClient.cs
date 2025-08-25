using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ClueClient : IClueSync
{
    public IEnumerator GetClue(Action<ClueSharedInfoList> onSuccess, Action<string> onError)
    {
        yield return null;
    }
    public IEnumerator PostClue(Action onSuccess, Action<string> onError)
    {
        yield return null;
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
        }
    }
    
}
