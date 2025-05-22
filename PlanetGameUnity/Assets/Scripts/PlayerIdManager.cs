using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdManager : MonoBehaviour
{
    static string filePath => Path.Combine(Application.persistentDataPath, "PlayerId.json");
    public static string Id { get; private set; }
    private void Awake()
    {
        Id = GetId(GetOrCreatePlayerId());
    }
    string GetId(PlayerId playerId)
    {
        return playerId.Id; 
    }
    public static PlayerId LoadId()
    {
        //�t�@�C���������json��Ԃ�
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<PlayerId>(json);
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// ��������ID��ۑ����郁�\�b�h
    /// </summary>
    /// <param name="id"></param>
    public static void SaveId(PlayerId id)
    {
        string json = JsonUtility.ToJson(id, true);
        File.WriteAllText(filePath, json);
    }
    public static PlayerId GetOrCreatePlayerId()
    {
        //Id��ǂݍ��݁Anull��������i����N�����Ȃǁj��
        //Id��V�K�쐬���A�ۑ�����
        PlayerId data = LoadId();
        if(data == null)
        {
            data = new PlayerId
            {
                //�����_�����A���ԁA�l�b�g���[�N���ȂǑ��l�ȗv�f�𗘗p����
                //���l������̂łقڈ�ӂȂ��̂ɂȂ�
                Id = System.Guid.NewGuid().ToString()
            };
            SaveId(data);
        }
        return data;
    }

    [System.Serializable]
    public class PlayerId
    {
        public string Id;
    }
}
