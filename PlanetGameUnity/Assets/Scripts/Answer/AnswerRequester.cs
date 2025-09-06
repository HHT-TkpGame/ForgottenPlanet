using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AnswerRequester
{
    /// <summary>
    /// ì¸óÕÇ≥ÇÍÇΩìöÇ¶Ç∆å©Ç¬ÇØÇΩéËÇ™Ç©ÇËÇÃêîÇéÊìæ
    /// </summary>
    /// <param name="onSuccess"></param>
    /// <param name="onError"></param>
    /// <returns></returns>
    public IEnumerator FetchAnswerAndFoundClues(Action<int, int> onSuccess = null, Action onError = null)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/answer";
        UnityWebRequest request = new UnityWebRequest(uri, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            AnswerData json = JsonUtility.FromJson<AnswerData>(request.downloadHandler.text);
            onSuccess?.Invoke(json.answer_id, json.found_clues);
        }
        else
        {
            Debug.Log(request.error);
            onError?.Invoke();
        }
    }
    /// <summary>
    /// ëIëÇµÇΩìöÇ¶ÇëóêM
    /// </summary>
    /// <param name="data"></param>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public IEnumerator PostAnswer(int answer, Action onSuccess = null, Action onError = null)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/answer";
        UnityWebRequest request = new UnityWebRequest(uri, "POST"); 
        AnswerData data = new AnswerData {answer_id = answer};
        string json = JsonUtility.ToJson(data);
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(rawData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            onSuccess?.Invoke();
        }
        else
        {
            Debug.Log(request.error);
            onError?.Invoke();
        }
    }
}
