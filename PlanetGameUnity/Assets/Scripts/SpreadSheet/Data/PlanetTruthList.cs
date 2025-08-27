using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName ="PlanetTruthList",menuName = "ScriptableObject/PlanetTruthList")]
public class PlanetTruthList : ScriptableObject
{
    public List<PlanetTruth> DataList;

    private void OnEnable()
    {
        //Debug.Log("LoadCSV");
        //LoadCsvData();
    }
    public void LoadCsvData()
    {
        DataList = new List<PlanetTruth>();

        // Resources �t�H���_���� CSV ��ǂݍ���
        // CSV �� Assets/Resources/SpreadSheet/Data/PlanetTruth.csv �ɒu��
        TextAsset csvAsset = Resources.Load<TextAsset>("PlanetTruth"); // �g���q�͕s�v
        if (csvAsset == null)
        {
            Debug.LogError("CSV�t�@�C����������܂���: PlanetTruth");
            return;
        }

        // CsvUtility ���g�����߂Ɉꎞ�I�Ƀ�������ɏ�������
        string tempPath = Path.Combine(Application.temporaryCachePath, "PlanetTruth_temp.csv");
        File.WriteAllText(tempPath, csvAsset.text);

        // CsvUtility �̓t�@�C���p�X����ǂݍ��ނ̂ŁA�ꎞ�t�@�C����n��
        string[,] data = CsvUtility.LoadCsvAs2DArray(tempPath);

        for (int i = 2; i < data.GetLength(0); i++)
        {
            var planetTruth = new PlanetTruth
            {
                Truth = int.Parse(data[i, 0]),
                TruthName = data[i, 1],
                IdNo1 = int.Parse(data[i, 2]),
                IdNo2 = int.Parse(data[i, 3]),
                IdNo3 = int.Parse(data[i, 4]),
                IdNo4 = int.Parse(data[i, 5]),
                IdNo5 = int.Parse(data[i, 6])
            };
            DataList.Add(planetTruth);
        }

        // �ꎞ�t�@�C���͍폜���Ă��ǂ�
        File.Delete(tempPath);
    }
    //public void LoadCsvData()
    //{
    //    DataList = new List<PlanetTruth>();
    //    //C:\Unity\PlanetGame\PlanetGameUnity\Assets\Scripts\SpreadSheet\Data
    //    //C:/Users/fanta/PlanetGame2/PlanetGameUnity/Assets/Scripts/SpreadSheet/Data/PlanetTruth.csv
    //    //var filePath ="C:/Unity/PlanetGame/PlanetGameUnity/Assets/Scripts/SpreadSheet/Data/PlanetTruth.csv";
    //    string filePath = Path.Combine(Application.dataPath, "Scripts/SpreadSheet/Data/PlanetTruth.csv");

    //    string[,] data = CsvUtility.LoadCsvAs2DArray(filePath);
    //    for (int i = 2; i < data.GetLength(0); i++)
    //    {
    //        var planetTruth = new PlanetTruth();
    //        planetTruth.Truth = int.Parse(data[i, 0]);
    //        planetTruth.TruthName = data[i, 1];
    //        planetTruth.IdNo1 = int.Parse(data[i, 2]);
    //        planetTruth.IdNo2 = int.Parse(data[i, 3]);
    //        planetTruth.IdNo3 = int.Parse(data[i, 4]);
    //        planetTruth.IdNo4 = int.Parse(data[i, 5]);
    //        planetTruth.IdNo5 = int.Parse(data[i, 6]);
    //        DataList.Add(planetTruth);
    //    }
    //}
}

