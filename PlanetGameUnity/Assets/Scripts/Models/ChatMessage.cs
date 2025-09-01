using UnityEngine;

//ChatDataをラップするためのクラス
[System.Serializable]
public class ChatDataList
{
    public ChatData[] messages;
}
[System.Serializable]
public class ChatData
{
    public string player_id;
    public string message;
    public string sent_at;
    public ChatData(string playerId, string message)
    {
        this.player_id = playerId;
        this.message = message;
        this.sent_at = "";
    }
}
