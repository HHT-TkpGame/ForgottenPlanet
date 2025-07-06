using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class RoleSelect : IRoleSelect
{
    /// <summary>
    /// �����ɂ���v���C���[�̑I�������擾
    /// ���Ԋu�Ŏ��s
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetSelections(Action<SelectionDataList> onSuccess = null, Action<string> onError = null)
    {
        string uri = ApiConfig.BASE_URI+ "/api/room/" + MatchingManager.RoomId + "/selections";
        UnityWebRequest request = new UnityWebRequest(uri, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            SelectionDataList data = JsonUtility.FromJson<SelectionDataList>(request.downloadHandler.text);
            onSuccess?.Invoke(data);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }
    /// <summary>
    /// �����̖�E�ƑI�����b�N�̏�Ԃ𑗐M
    /// </summary>
    /// <param name="data"></param>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public IEnumerator PostSelection(
        SelectionData data,
        Action onSuccess = null,
        Action<string> onError = null
    )
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/selections";
        string json = JsonUtility.ToJson(data);
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(uri,"POST");
        request.uploadHandler = new UploadHandlerRaw(rawData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke();// �����R�[���o�b�N�����s
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            onError?.Invoke(request.error);// ���s�R�[���o�b�N�����s
            Debug.Log($"PostSelection failed: {request.error}");
        }
    }
    /// <summary>
    /// �����̖�E�𑗐M
    /// ���Ԋu�Ŏ��s
    /// </summary>
    /// <param name="data"></param>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public IEnumerator PostRole(SelectionData data, Action onSuccess = null, Action<string> onError = null)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/role";
        string json = JsonUtility.ToJson(data);
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(uri, "POST");
        request.uploadHandler = new UploadHandlerRaw(rawData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke();// �����R�[���o�b�N�����s
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            onError?.Invoke(request.error);// ���s�R�[���o�b�N�����s
            Debug.Log($"PostSelection failed: {request.error}");
        }
    }
    /// <summary>
    /// �S�����L�������肵�Ă����ԂŃz�X�g�̂ݑ���\�ŁA
    /// ��E������Ă��邩�擾
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
            SelectionData data = JsonUtility.FromJson<SelectionData>(request.downloadHandler.text);
            onSuccess?.Invoke(data.has_conflict);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }

    /// <summary>
    /// ��E������Ă��Ȃ��Ă���E�đI���������Ƃ���
    /// ���N�G�X�g�𑗐M����
    /// </summary>
    /// <returns></returns>
    public IEnumerator PostReselection(Action onSuccess = null, Action<string> onError = null)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/roles/reselection";
        UnityWebRequest request = new UnityWebRequest(uri, "POST");
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
    /// <summary>
    /// ���ɐi�߂郊�N�G�X�g
    /// </summary>
    /// <param name="onSuccess"></param>
    /// <param name="onError"></param>
    /// <returns></returns>
    public IEnumerator PostProcessToNextPhase(Action onSuccess = null, Action<string> onError = null)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/process";
        UnityWebRequest request = new UnityWebRequest(uri, "POST");
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
    /// <summary>
    /// ���ɐi��ł��������擾
    /// </summary>
    /// <param name="onSuccess"></param>
    /// <param name="onError"></param>
    /// <returns></returns>
    public IEnumerator GetCanProcessToNextPhase(Action onSuccess = null, Action<string> onError = null)
    {
        string uri = ApiConfig.BASE_URI + "/api/room/" + MatchingManager.RoomId + "/process";
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
