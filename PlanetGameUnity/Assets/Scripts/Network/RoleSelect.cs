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
    /// �����ɂ���v���C���[�̑I�������擾
    /// ����I�ɌĂяo��
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetSelection()
    {
        yield return null;
    }
    /// <summary>
    /// �����̖�E�ƑI�����b�N�̏�Ԃ𑗐M
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
    /// �S�����L�������肵�Ă����ԂŃz�X�g��������\�ȃ{�^���ŁA
    /// ��E������Ă��邩�擾
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetHasConflict()
    {
        yield return null;
    }

    /// <summary>
    /// ��E������Ă��Ȃ��Ă���E�đI���������Ƃ���
    /// ���N�G�X�g�𑗐M����
    /// </summary>
    /// <returns></returns>
    public IEnumerator PostReselection()
    {
        yield return null;
    }
}
