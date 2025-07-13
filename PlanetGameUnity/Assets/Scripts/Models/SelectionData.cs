[System.Serializable]
public class SelectionData
{
    public int room_id;
    public string player_id;
    public bool is_locked;//null許容
    public bool has_conflict;
    public bool is_commander;
    /// <summary>
    /// 選択情報送信用コンストラクタ
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="isLocked"></param>
    /// <param name="isCommander"></param>
    public SelectionData(string playerId, bool isLocked,bool isCommander)
    {
        player_id = playerId;
        is_locked = isLocked;
        is_commander = isCommander;
    }
    /// <summary>
    /// 役職送信用コンストラクタ
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="isCommander"></param>
    public SelectionData(string playerId, bool isCommander)
    {
        player_id = playerId;
        is_commander = isCommander;
    }
}
//RoleDataをラップするためのクラス
[System.Serializable]
public class SelectionDataList
{
    public SelectionData[] selections;
}

