using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class RoleSelect : IRoleSelect
{
    public bool HasConflict { get; private set; }
    public bool IsReselection { get; private set; }
    public bool IsHostButtonLocked { get; private set; }
    public bool IsGuestButtonLocked { get; private set; }
    /// <summary>
    /// 部屋にいるプレイヤーの選択情報を取得
    /// 定期的に呼び出す
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetSelection()
    {
        yield return null;
    }
    /// <summary>
    /// 自分の役職と選択ロックの状態を送信
    /// </summary>
    /// <param name="data"></param>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public IEnumerator PostRole(RoleData data)
    {
        string uri = ApiConfig.BASE_URI + MatchingManager.RoomId + "roles";
        string json = JsonUtility.ToJson(data);
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(uri,"POST");
        request.uploadHandler = new UploadHandlerRaw(rawData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.Log($"PostSelection failed: {request.error}");
        }
    }
    /// <summary>
    /// 全員がキャラ決定している状態でホストだけ操作可能なボタンで、
    /// 役職が被っているか取得
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetHasConflict()
    {
        yield return null;
    }

    /// <summary>
    /// 役職が被っていなくても役職再選択したいときに
    /// リクエストを送信する
    /// </summary>
    /// <returns></returns>
    public IEnumerator PostReselection()
    {
        yield return null;
    }
}
