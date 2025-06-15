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
        var filePath="C:/Users/fanta/PlanetGame2/PlanetGameUnity/Assets/Scripts/SpreadSheet/PlanetTruth.csv";
        string[,] data = GenerateScriptableObjectMenu.LoadCsvAs2DArray(filePath);
        for (int i = 2; i < data.GetLength(0); i++)
        {
            var planetTruth = new PlanetTruth();
            planetTruth.Truth = int.Parse(data[i, 0]);
            planetTruth.TruthName = data[i, 1];
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
}

