using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ClueSync : IClueSync
{
    public IEnumerator GetClue(Action<ClueData> onSuccess, Action<string> onError)
    {
        yield return null;
    }
    public IEnumerator PostClue(Action onSuccess, Action<string> onError)
    {
        yield return null;
    }
    /// <summary>
    /// �}�b�`�J�n���ɍ���̐^��ID�Ǝ肪����͈̔͂��擾����
    /// </summary>
    /// <param name="onSuccess"></param>
    /// <param name="onError"></param>
    /// <returns></returns>
    public IEnumerator GetClueAndTruth(Action<ClueData> onSuccess, Action<string> onError)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/clueAndTruth";
        UnityWebRequest request = new UnityWebRequest(uri, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            ClueData json = JsonUtility.FromJson<ClueData>(request.downloadHandler.text);
            onSuccess?.Invoke(json);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }
    
}
