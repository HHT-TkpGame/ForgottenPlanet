[System.Serializable]
public class RoleData
{
    public int room_id;
    public string player_id;
    public bool is_locked;
    public bool has_conflict;
    public bool is_commander;
    public RoleData(string playerId, bool isLocked,bool isCommander)
    {
        player_id = playerId;
        is_locked = isLocked;
        is_commander = isCommander;
    }
}
//RoleData�����b�v���邽�߂̃N���X
[System.Serializable]
public class RoleDataList
{
    public RoleData[] Selections;
}

