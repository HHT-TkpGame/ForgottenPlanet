[System.Serializable]
public class RoleData
{
    public int room_id;
    public string player_id;
    public bool is_rocked;
    public bool has_conflict;
    public RoleData(string playerId, bool isLocked)
    {
        player_id = playerId;
        is_rocked = isLocked;
    }
}
//RoleData�����b�v���邽�߂̃N���X
[System.Serializable]
class RoleDataList
{
    public RoleData[] Selections;
}

