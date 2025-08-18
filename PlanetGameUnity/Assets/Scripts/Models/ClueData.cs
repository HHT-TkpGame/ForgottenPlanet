using UnityEngine;

[System.Serializable]
public class ClueData
{
    public int rnd;
    public ClueBehavior clue;
}

[System.Serializable]
public class Truth
{
    public int truth;
    public string truthName;
    public Truth(int truth, string truthName)
    {
        this.truth = truth;
        this.truthName = truthName;
    }
}