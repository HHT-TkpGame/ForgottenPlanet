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

	//unity上部ウィンドウの部分がこれで増える
	[MenuItem("Tool/GenerateScriptableObjectMenu")]

	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(GenerateScriptableObjectMenu));
	}

	private void OnGUI()
	{
		//textFieldでGASのURLと自動生成するScriptableObjectの名前を入力
		gasUrl = EditorGUILayout.TextField("gasUrl", gasUrl);
		scriptableObjectName = EditorGUILayout.TextField("scriptableObjectName", scriptableObjectName);

		//GASから取得したCSVデータを保存するファイル名と自動生成するScriptableObjectのスクリプトとパスを設定
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

				//先頭の\uFEFFを削除
				if (csvData[0] == '\uFEFF')
				{
					csvData = csvData.Substring(1);
				}

				//CSVファイルとして保存
				SaveCsvFile(csvData);

				//GASから受け取ったCSVデータを2次元配列に変換
				var ParseCsvData = ParseCsv(csvData);

				//ScriptableObjectのc#スクリプトを生成
				GenerateScriptableObjectCS(ParseCsvData);
			}
		}
	}
	//CSVファイルに保存
	void SaveCsvFile(string data)
	{
		File.WriteAllText(outPutCsvFilePath, data, Encoding.UTF8);
		AssetDatabase.Refresh();
	}
	//2次元配列でCSVファイルをロードする関数
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

	//CSVデータを2次元配列に変換する関数
	string[,] ParseCsv(string csvData)
	{
		//行ごとに分割(\r\nか\nを考慮)
		string[] rows = csvData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

		//1行目の列数を基準にする
		string[] firstRow = SplitCsvLine(rows[0]);
		int rowCount = rows.Length;
		int colCount = firstRow.Length;

		//2次元配列を作成
		string[,] result = new string[rowCount, colCount];

		for (int i = 0; i < rowCount; i++)
		{
			string[] cols = SplitCsvLine(rows[i]);

			for (int j = 0; j < colCount; j++)
			{
				//配列の範囲を越えた時空文字をセット
				result[i, j] = j < cols.Length ? cols[j] : "";
			}
		}

		return result;
	}
	//CSVの1行を分割する関数
	string[] SplitCsvLine(string line)
	{
		//正規表現でのCSVフィールドを抽出
		MatchCollection matches = Regex.Matches(line, "\"([^\"]*)\"|([^,]+)");

		string[] fields = new string[matches.Count];

		for (int i = 0; i < matches.Count; i++)
		{
			//ダブルクォートを削除
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
				// スプレッドシートの1行目(変数名)、２行目(型)を参照し、変数を作成
				sw.WriteLine("    [SerializeField]");
				sw.WriteLine($"    public {ParseCsvData[1, i]} {ParseCsvData[0, i]};");
			}
			sw.WriteLine("}\n");
		}
		AssetDatabase.Refresh();
	}

}
