[System.Serializable]
public class RoleData
{
    public int room_id;
    public string player_id;
    public bool is_rocked;
    public bool has_conflict;
    public RoleData(int roomId, string playerId, bool isLocked, bool hasConflict)
    {
        room_id = roomId;
        player_id = playerId;
        is_rocked = isLocked;
        has_conflict = hasConflict;
    }
}
//RoleDataをラップするためのクラス
[System.Serializable]
class RoleDataList
{
    public RoleData[] Selections;
}

