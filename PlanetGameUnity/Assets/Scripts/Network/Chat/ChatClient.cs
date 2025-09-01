using System.Text;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ChatClient
{
    const string BASE_URL = ApiConfig.BASE_URI;
    DateTime lastFetchAt;
    /// <summary>
    /// �T�[�o�[�Ƀ`���b�g�𑗂�
    /// </summary>
    /// <param name="uri">API�܂ł�URL</param>
    /// <param name="message">�`���b�g�̓��e</param>
    /// <returns></returns>
    public IEnumerator SendChatMessage(string message, Action<string> onSuccess, Action onError)
    {
        string uri = $"{BASE_URL}/api/room/{MatchingManager.RoomId}/chat/";
        UnityWebRequest request = new UnityWebRequest(uri, "POST");
        string json = JsonUtility.ToJson(new ChatData(PlayerIdManager.Id, message));
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(rawData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke(request.downloadHandler.text);
        }
        else
        {
            onError?.Invoke();
            Debug.Log(request.error);
        }
    }

    //�`���b�g���擾����
    public IEnumerator FetchChats(Action<ChatDataList> onSuccess, Action onError)
    { 
        //ToString("o")�ŃT�[�o�[����date�^�o���f�[�V�������ʂ�悤�ɂ���
        string reqUri = $"{BASE_URL}/api/room/{MatchingManager.RoomId}/chat/?since={UnityWebRequest.EscapeURL(lastFetchAt.ToString("o"))}&player_id={UnityWebRequest.EscapeURL(PlayerIdManager.Id)}";
        UnityWebRequest request = new UnityWebRequest(reqUri, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            ChatDataList resJson = JsonUtility.FromJson<ChatDataList>(request.downloadHandler.text);
            if (resJson.messages.Length != 0 && resJson != null)
            {
                // �󂯎�����ŐV�̃��b�Z�[�W�� sent_at ���X�V
                string latestSentAt = resJson.messages[^1].sent_at;
                if (DateTime.TryParse(latestSentAt, out DateTime latest))
                {
                    lastFetchAt = latest;
                }
            }
            onSuccess?.Invoke(resJson);
        }
        else
        {
            Debug.Log(request.error);
            onError?.Invoke();
        }
    }
}
