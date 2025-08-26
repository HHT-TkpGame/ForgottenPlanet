using UnityEngine;

//�T�[�o�[����󂯎�����肪���菉�����p�̃f�[�^�N���X
[System.Serializable]
public class ServerCurrentMatchClues
{
    public int truth_id;
    public int[] clues_range = new int[2];
}
//ClueSharedInfo�����b�v����N���X
[System.Serializable]
public class ClueSharedInfoList
{
    public ClueSharedInfo[] shared_clues;
}
//�T�[�o�[����󂯎��肪���苤�L���̃N���X
[System.Serializable]
public class ClueSharedInfo
{
    public int clue_id;
    public bool is_shared;
}

//�}�b�v��̃I�u�W�F�N�g�Ɏ肪������������邽�߂̃f�[�^
public class ClueData
{
    public int rnd;
    public ClueBehavior clue;
}
//����̃}�b�`�̎肪����f�[�^
public class CurrentMatchClues
{
    public int truthId;
    public int[] clueIds;
    public bool[] isShared;
    public CurrentMatchClues(int truthId, int rangeStart, int rangeEnd)
    {
        this.truthId = truthId;
        int maxClues = rangeEnd - rangeStart + 1;
        clueIds = new int[maxClues];
        isShared = new bool[maxClues];
        for(int i = 0; i < maxClues; i++)
        {
            clueIds[i] = rangeStart + i;
        }
    }
}

//CSV�̈�s���̃f�[�^������
[System.Serializable]
public class PlanetTruth
{
    public int Truth;
    public string TruthName;
    public int IdNo1;
    public int IdNo2;
    public int IdNo3;
    public int IdNo4;
    public int IdNo5;
}