using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlanetTruthList",menuName = "ScriptableObject/PlanetTruthList")]
public class PlanetTruthList : ScriptableObject
{
    [SerializeField]
    public List<PlanetTruth> DataList;

    private void OnEnable()
    {
        LoadCsvData();
    }
    public void LoadCsvData()
    {
        DataList = new List<PlanetTruth>();
        var filePath="C:/Users/fanta/PlanetGame2/PlanetGameUnity/Assets/Scripts/SpreadSheet/Data/PlanetTruth.csv";
        string[,] data = CsvUtility.LoadCsvAs2DArray(filePath);
        for (int i = 2; i < data.GetLength(0); i++)
        {
            var planetTruth = new PlanetTruth();
            planetTruth.Truth = int.Parse(data[i, 0]);
            planetTruth.TruthName = data[i, 1];
            planetTruth.IdNo1 = int.Parse(data[i, 2]);
            planetTruth.IdNo2 = int.Parse(data[i, 3]);
            planetTruth.IdNo3 = int.Parse(data[i, 4]);
            planetTruth.IdNo4 = int.Parse(data[i, 5]);
            planetTruth.IdNo5 = int.Parse(data[i, 6]);
            DataList.Add(planetTruth);
        }
    }
}

[System.Serializable]
public class PlanetTruth
{
    [SerializeField]
    public int Truth;
    [SerializeField]
    public string TruthName;
    [SerializeField]
    public int IdNo1;
    [SerializeField]
    public int IdNo2;
    [SerializeField]
    public int IdNo3;
    [SerializeField]
    public int IdNo4;
    [SerializeField]
    public int IdNo5;
}

