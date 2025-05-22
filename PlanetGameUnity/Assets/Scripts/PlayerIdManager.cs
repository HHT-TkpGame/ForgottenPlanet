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
        //ファイルがあればjsonを返す
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
    /// 生成したIDを保存するメソッド
    /// </summary>
    /// <param name="id"></param>
    public static void SaveId(PlayerId id)
    {
        string json = JsonUtility.ToJson(id, true);
        File.WriteAllText(filePath, json);
    }
    public static PlayerId GetOrCreatePlayerId()
    {
        //Idを読み込み、nullだったら（初回起動時など）ら
        //Idを新規作成し、保存する
        PlayerId data = LoadId();
        if(data == null)
        {
            data = new PlayerId
            {
                //ランダム性、時間、ネットワーク情報など多様な要素を利用した
                //数値が入るのでほぼ一意なものになる
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
