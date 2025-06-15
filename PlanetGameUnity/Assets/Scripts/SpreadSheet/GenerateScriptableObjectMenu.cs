using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class GenerateScriptableObjectMenu : EditorWindow
{
	string gasUrl = "";
	string scriptableObjectName = "";
	string outPutCsvFilePath = "";
	string outPutCSFilePath = "";

	//unity�㕔�E�B���h�E�̕���������ő�����
	[MenuItem("Tool/GenerateScriptableObjectMenu")]

	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(GenerateScriptableObjectMenu));
	}

	private void OnGUI()
	{
		//textField��GAS��URL�Ǝ�����������ScriptableObject�̖��O�����
		gasUrl = EditorGUILayout.TextField("gasUrl", gasUrl);
		scriptableObjectName = EditorGUILayout.TextField("scriptableObjectName", scriptableObjectName);

		//GAS����擾����CSV�f�[�^��ۑ�����t�@�C�����Ǝ�����������ScriptableObject�̃X�N���v�g�ƃp�X��ݒ�
		outPutCsvFilePath = Application.dataPath + "/Scripts/" + scriptableObjectName + ".csv";
		outPutCSFilePath = Application.dataPath + "/Scripts/" + scriptableObjectName + ".cs";

		if (GUILayout.Button("GenerateScriptableObject"))
		{
			EditorCoroutineUtility.StartCoroutine(GenerateScriptableObject(), this);
		}
	}
	IEnumerator GenerateScriptableObject()
	{
		using (UnityWebRequest request = UnityWebRequest.Get(gasUrl))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.Success)
			{
				var csvData = request.downloadHandler.text;

				//�擪��\uFEFF���폜
				if (csvData[0] == '\uFEFF')
				{
					csvData = csvData.Substring(1);
				}

				//CSV�t�@�C���Ƃ��ĕۑ�
				SaveCsvFile(csvData);

				//GAS����󂯎����CSV�f�[�^��2�����z��ɕϊ�
				var ParseCsvData = ParseCsv(csvData);

				//ScriptableObject��c#�X�N���v�g�𐶐�
				GenerateScriptableObjectCS(ParseCsvData);
			}
		}
	}
	//CSV�t�@�C���ɕۑ�
	void SaveCsvFile(string data)
	{
		File.WriteAllText(outPutCsvFilePath, data, Encoding.UTF8);
		AssetDatabase.Refresh();
	}
	//2�����z���CSV�t�@�C�������[�h����֐�
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

	//CSV�f�[�^��2�����z��ɕϊ�����֐�
	string[,] ParseCsv(string csvData)
	{
		//�s���Ƃɕ���(\r\n��\n���l��)
		string[] rows = csvData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

		//1�s�ڂ̗񐔂���ɂ���
		string[] firstRow = SplitCsvLine(rows[0]);
		int rowCount = rows.Length;
		int colCount = firstRow.Length;

		//2�����z����쐬
		string[,] result = new string[rowCount, colCount];

		for (int i = 0; i < rowCount; i++)
		{
			string[] cols = SplitCsvLine(rows[i]);

			for (int j = 0; j < colCount; j++)
			{
				//�z��͈̔͂��z�������󕶎����Z�b�g
				result[i, j] = j < cols.Length ? cols[j] : "";
			}
		}

		return result;
	}
	//CSV��1�s�𕪊�����֐�
	string[] SplitCsvLine(string line)
	{
		//���K�\���ł�CSV�t�B�[���h�𒊏o
		MatchCollection matches = Regex.Matches(line, "\"([^\"]*)\"|([^,]+)");

		string[] fields = new string[matches.Count];

		for (int i = 0; i < matches.Count; i++)
		{
			//�_�u���N�H�[�g���폜
			fields[i] = matches[i].Value.Trim('"');
		}

		return fields;
	}

	void GenerateScriptableObjectCS(string[,] ParseCsvData)
	{
		using (var sw = new StreamWriter(outPutCSFilePath))
		{
			sw.WriteLine("using System.Collections.Generic;");
			sw.WriteLine("using UnityEngine;\n");
			sw.WriteLine($"[CreateAssetMenu(fileName =\"{scriptableObjectName}List\",menuName = \"ScriptableObject/{scriptableObjectName}List\")]");
			sw.WriteLine($"public class {scriptableObjectName}List : ScriptableObject");
			sw.WriteLine("{");
			sw.WriteLine("    [SerializeField]");
			sw.WriteLine($"    public List<{scriptableObjectName}> DataList;\n");
			sw.WriteLine($"    private void OnEnable()");
			sw.WriteLine("    {");
			sw.WriteLine("        LoadCsvData();");
			sw.WriteLine("    }");
			sw.WriteLine("    public void LoadCsvData()");
			sw.WriteLine("    {");
			sw.WriteLine($"        DataList = new List<{scriptableObjectName}>();");
			sw.WriteLine($"        var filePath=\"{outPutCsvFilePath}\";");
			sw.WriteLine("        string[,] data = GenerateScriptableObjectMenu.LoadCsvAs2DArray(filePath);");
			sw.WriteLine("        for (int i = 2; i < data.GetLength(0); i++)");
			sw.WriteLine("        {");

			var dataName = char.ToLower(scriptableObjectName[0]) + scriptableObjectName.Substring(1);
			sw.WriteLine($"            var {dataName} = new {scriptableObjectName}();");

			for (int i = 0; i < ParseCsvData.GetLength(1); i++)
			{
				var parseDatastr = ParseCsvData[1, i] == "string" ? $"data[i, {i}]" : $"{ParseCsvData[1, i]}.Parse(data[i, {i}])";
				sw.WriteLine($"            {dataName}.{ParseCsvData[0, i]} = {parseDatastr};");
			}

			sw.WriteLine($"            DataList.Add({dataName});");
			sw.WriteLine("        }");
			sw.WriteLine("    }");
			sw.WriteLine("}\n");

			sw.WriteLine("[System.Serializable]");
			sw.WriteLine($"public class {scriptableObjectName}");
			sw.WriteLine("{");
			for (int i = 0; i < ParseCsvData.GetLength(1); i++)
			{
				// �X�v���b�h�V�[�g��1�s��(�ϐ���)�A�Q�s��(�^)���Q�Ƃ��A�ϐ����쐬
				sw.WriteLine("    [SerializeField]");
				sw.WriteLine($"    public {ParseCsvData[1, i]} {ParseCsvData[0, i]};");
			}
			sw.WriteLine("}\n");
		}
		AssetDatabase.Refresh();
	}

}
