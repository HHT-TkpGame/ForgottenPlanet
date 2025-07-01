using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class RoleSelect : IRoleSelect
{
    /// <summary>
    /// 部屋にいるプレイヤーの選択情報を取得
    /// 定期的に呼び出す
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetSelection(Action<RoleDataList> onSuccess = null, Action<string> onError = null)
    {
        string uri = ApiConfig.BASE_URI+ "/api/room/" + MatchingManager.RoomId + "/roles";
        UnityWebRequest request = new UnityWebRequest(uri, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            RoleDataList data = JsonUtility.FromJson<RoleDataList>(request.downloadHandler.text);
            onSuccess?.Invoke(data);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }
    /// <summary>
    /// 自分の役職と選択ロックの状態を送信
    /// </summary>
    /// <param name="data"></param>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public IEnumerator PostRole(RoleData data, Action onSuccess = null, Action<string> onError = null)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/roles";
        string json = JsonUtility.ToJson(data);
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(uri,"POST");
        request.uploadHandler = new UploadHandlerRaw(rawData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke();// 成功コールバックを実行
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            onError?.Invoke(request.error);// 失敗コールバックを実行
            Debug.Log($"PostSelection failed: {request.error}");
        }
    }
    /// <summary>
    /// 全員がキャラ決定している状態でホストのみ操作可能で、
    /// 役職が被っているか取得
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetHasConflict(Action<bool> onSuccess = null, Action<string> onError = null)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/roles/conflict";
        UnityWebRequest request = new UnityWebRequest(uri, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.Success)
        {
            RoleData data = JsonUtility.FromJson<RoleData>(request.downloadHandler.text);
            onSuccess?.Invoke(data.has_conflict);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }

    /// <summary>
    /// 役職が被っていなくても役職再選択したいときに
    /// リクエストを送信する
    /// </summary>
    /// <returns></returns>
    public IEnumerator PostReselection(Action onSuccess = null, Action<string> onError = null)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/roles/reselection";
        UnityWebRequest request = new UnityWebRequest(uri, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke();
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }
}
