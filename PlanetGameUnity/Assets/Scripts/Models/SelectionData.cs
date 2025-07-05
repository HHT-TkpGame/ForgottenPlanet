[System.Serializable]
public class SelectionData
{
    public int room_id;
    public string player_id;
    public bool is_locked;//null���e
    public bool has_conflict;
    public bool is_commander;
    /// <summary>
    /// �I����񑗐M�p�R���X�g���N�^
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
    /// ��E���M�p�R���X�g���N�^
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="isCommander"></param>
    public SelectionData(string playerId, bool isCommander)
    {
        player_id = playerId;
        is_commander = isCommander;
    }
}
//RoleData�����b�v���邽�߂̃N���X
[System.Serializable]
public class SelectionDataList
{
    public SelectionData[] selections;
}

