using UnityEngine;

//サーバーから受け取った手がかり初期化用のデータクラス
[System.Serializable]
public class ServerCurrentMatchClues
{
    public int truth_id;
    public int[] clues_range = new int[2];
}
//ClueSharedInfoをラップするクラス
[System.Serializable]
public class ClueSharedInfoList
{
    public ClueSharedInfo[] shared_clues;
}
//サーバーから受け取る手がかり共有情報のクラス
[System.Serializable]
public class ClueSharedInfo
{
    public int clue_id;
    public bool is_shared;
}

//マップ上のオブジェクトに手がかりを持たせるためのデータ
public class ClueData
{
    public int rnd;
    public ClueBehavior clue;
}
//今回のマッチの手がかりデータ
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

//CSVの一行分のデータが入る
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