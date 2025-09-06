using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameStateRequester
{
    /// <summary>
    /// ���݂̃Q�[���i�s���擾
    /// </summary>
    /// <param name="onSuccess"></param>
    /// <param name="onError"></param>
    /// <returns></returns>
    public IEnumerator GetState(Action<GameStateData> onSuccess = null, Action<string> onError = null)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/progress";
        UnityWebRequest request = new UnityWebRequest(uri, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            GameStateData json = JsonUtility.FromJson<GameStateData>(request.downloadHandler.text);
            if (json.game_progress != -1)
            {
                onSuccess?.Invoke(json);
            }
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }
    /// <summary>
    /// �Q�[���i�s�̍X�V���N�G�X�g
    /// </summary>
    /// <param name="data"></param>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public IEnumerator PostState(Action<GameStateData> onSuccess = null, Action<string> onError = null)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/progress";
        UnityWebRequest request = new UnityWebRequest(uri, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            GameStateData json = JsonUtility.FromJson<GameStateData>(request.downloadHandler.text);
            onSuccess?.Invoke(json);
        }
        else
        {
            onError?.Invoke(request.error);// ���s�R�[���o�b�N�����s
            Debug.Log($"PostState: {request.error}");
        }
    }
}
