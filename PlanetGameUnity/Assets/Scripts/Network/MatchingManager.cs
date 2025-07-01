using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MatchingManager : MonoBehaviour
{
    TMP_InputField inputField;
    const string MATCH_API_ENDPOINT = "/api/match";
    
    
    void Start()
    {
        inputField = GameObject.Find("Keyword").GetComponent<TMP_InputField>();
    }
    void Update()
    {
        
    }
    bool isWaiting;//�}�b�`�ҋ@����
    const int CHARACTER_LIMIT = 10;//�����t�̕��������
    /// <summary>
    /// InputField����ǂݎ���������t�ŕ�����T��
    /// </summary>
    public void SendKeyword()
    {
        if(inputField.text.Length == 0 && inputField.text.Length <= CHARACTER_LIMIT)
        {
            Debug.Log("���̓G���[");
            return;
        }
        //�}�b�`�ҋ@���Ȃ���s���Ȃ�
        if (!isWaiting)
        {
            StartCoroutine(FindRoom(inputField.text, PlayerIdManager.Id, ApiConfig.BASE_URI + MATCH_API_ENDPOINT));
        }
    }
    const float REQUEST_INTERVAL = 1f;
    /// <summary>
    /// GetPlayerCountInRoom()�����Ԋu�ŌĂ�
    /// </summary>
    /// <returns></returns>
    IEnumerator GetPlayerCountInRoomLoop()
    {
        while (isWaiting) 
        {
            yield return StartCoroutine(GetPlayerCountInRoom(ApiConfig.BASE_URI + "/api/room/" + RoomId + "/playerCount"));
            Debug.Log("�}�b�`�ҋ@��");
            yield return new WaitForSeconds(REQUEST_INTERVAL);
        }
    }
    const int MAX_PLAYER_COUNT = 2;
    /// <summary>
    /// �Q�����������̐l�����擾���A2�l��������V�[���J�ڂ���
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    IEnumerator GetPlayerCountInRoom(string uri)
    {
        UnityWebRequest request = new UnityWebRequest(uri, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            PlayerCount res = JsonUtility.FromJson<PlayerCount>(json);
            Debug.Log(res.player_count);
            if(res.player_count == MAX_PLAYER_COUNT)
            {
                SceneChangeManager.SceneChange("RoleSetScene");
            }
        }
        else
        {
            Debug.Log("Error" + request.error);
        }
    }

    /// <summary>
    /// ���͂��ꂽ�����t�ŕ�����T���A�쐬or�Q������
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="playerId"></param>
    /// <param name="uri"></param>
    /// <returns></returns>
    IEnumerator FindRoom(string keyword, string playerId, string uri)
    {
        //���N�G�X�g�̃{�f�B
        string json = JsonUtility.ToJson(new MatchData(keyword, playerId));
        byte[] rawData = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(uri, "POST");
        request.uploadHandler = new UploadHandlerRaw(rawData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        isWaiting = true;
        //���ʂ��Ԃ��ė���܂őҋ@
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            NetworkStateManager.SetState(NetworkStateManager.NetworkState.Connected);
            Debug.Log("Success" + request.downloadHandler.text);
            string res = request.downloadHandler.text;
            RoomData roomData = JsonUtility.FromJson<RoomData>(res);
            RoomId = roomData.room_id;
            IsHost = roomData.is_host;
            Debug.Log("RoomId : " + roomData.room_id + "isHost" + roomData.is_host);
            
            //�����̃v���C���[�̐������Ԋu�Ō��ɍs��
            StartCoroutine(GetPlayerCountInRoomLoop());
        }
        else if (request.responseCode == 403)
        {
            isWaiting = false;
            Debug.Log("Full : "+request.downloadHandler.text);
        }
        else
        {
            isWaiting = false;
            Debug.Log("Error" + request.error);
        }
    }
    [System.Serializable]
    class MatchData
    {
        public string keyword;//�}�b�`���O�Ɏg�������t
        public string player_id;//������ID

        public MatchData(string keyword, string player_id)
        {
            this.keyword = keyword;
            this.player_id = player_id;
        }
    }
    [System.Serializable]
    class RoomData
    {
        public int room_id;
        public bool is_host;
    }
    public static bool IsHost { get; private set; }
    public static bool IsCommander { get; set; }
    public static int RoomId { get; private set; }
    
    [System.Serializable]
    class PlayerCount
    {
        public int player_count;//�����̓����Ă��镔���̐l��
    }
}
