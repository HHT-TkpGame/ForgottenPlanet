using System.IO;
using UnityEngine;

public static class CsvUtility 
{
	public static string[,] LoadCsvAs2DArray(string filePath)
	{
		//行ごとに読み込む
		string[] lines = File.ReadAllLines(filePath);
		int rows = lines.Length;
		int cols = lines[0].Split(',').Length;
		string[,] array = new string[rows, cols];

		for (int i = 0; i < rows; i++)
		{
			//カンマ区切りで分割
			string[] cells = lines[i].Split(",");
			for (int j = 0; j < cols; j++)
			{
				//余分な"を削除
				array[i, j] = cells[j].Trim('\"');
			}
		}

		return array;
	}
}
