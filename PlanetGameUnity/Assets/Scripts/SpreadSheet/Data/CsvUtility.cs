using System.IO;
using UnityEngine;

public static class CsvUtility 
{
	public static string[,] LoadCsvAs2DArray(string filePath)
	{
		//�s���Ƃɓǂݍ���
		string[] lines = File.ReadAllLines(filePath);
		int rows = lines.Length;
		int cols = lines[0].Split(',').Length;
		string[,] array = new string[rows, cols];

		for (int i = 0; i < rows; i++)
		{
			//�J���}��؂�ŕ���
			string[] cells = lines[i].Split(",");
			for (int j = 0; j < cols; j++)
			{
				//�]����"���폜
				array[i, j] = cells[j].Trim('\"');
			}
		}

		return array;
	}
}
