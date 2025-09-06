using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class MonitorController : MonoBehaviour
{
    const int MAXCLUES = 5;
    [SerializeField, Header("Fileの配列")] FileBehavior[] files=new FileBehavior[MAXCLUES];
    [SerializeField, Header("InfoのPanelの配列")] GameObject[] infoPanels=new GameObject[MAXCLUES];

 //   [SerializeField, Header("AIの暴走の配列")]
 //   Sprite[] truth1Array;
	//[SerializeField, Header("異星人の配列")]
	//Sprite[] truth2Array;
	//[SerializeField, Header("異常気象の配列")]
	//Sprite[] truth3Array;
	//[SerializeField, Header("パンデミックの配列")]
	//Sprite[] truth4Array;
	//[SerializeField, Header("内乱の配列")]
	//Sprite[] truth5Array;
	//[SerializeField, Header("次元の配列")]
	//Sprite[] truth6Array;

	[SerializeField] CodeController codeController;
	[SerializeField] InputFieldManager inputManager;

	//表示されているPanelを入れる配列
	GameObject panelObj;

	CluesManager cluesManager;

	int truth;
    int range;

    const int AI_RAMPAGE = 0;
	const int ALIEN_INVASION = 1;
	const int EXTREME_WEATHER= 2;
	const int PANDEMIC = 3;
	const int POLITICAL_TURMOIL = 4;
	const int DIMENSIONS = 5;

	//その時の真相に合わせて使う配列
	Sprite[] truthArray;

	List<ClueSharedInfo> clueGettingStates=new List<ClueSharedInfo>();

	[SerializeField]int testTruth;
	[SerializeField]int[] testTruthRange = new int[5];

	[SerializeField]CluesTextSetter setter;
	[SerializeField] Sprite clueImageOutline;

	
	public void Init(CluesManager cluesManager)
	{
		this.cluesManager = cluesManager;

		truth = 1;

		codeController.SetCodeList();

		//ここの値がサーバーのやつ
		setter.SetText(testTruth, testTruthRange);

		FileSetup();
		PanelSetup();

	}

void Start()
	{
		truth = 1;

		codeController.SetCodeList();
		setter.SetText(testTruth, testTruthRange);

		FileSetup();
		//TruthSetup();
		PanelSetup();

	}

	public void UpdateUI(List<ClueSharedInfo> clues)
	{
		foreach (ClueSharedInfo updated in clues)
		{
			
			// MatchClues.clueIds の中から対象の clue_id のインデックスを探す
			//インデックスに帰ってくるものはリストの何番目にあるか
			int index = System.Array.IndexOf(cluesManager.MatchClues.clueIds, updated.clue_id);

			Debug.Log("MatchClues.clueIds"+cluesManager.MatchClues.clueIds[0]);
			Debug.Log("index"+index+"clueId"+updated.clue_id);
			
			//見つからなかったときindexが-1になるから入れたほうがいい気がする
			//if (index < 0) { return; }

			//一致したIdのローカルのリストのis_sharedがTrueなら
			if (clueGettingStates[index].is_shared)
			{
				//UI更新
				//配列の要素番号はゼロからだけどClueIdは1からだからその理由から-1
				files[index].ActiveInteractable();
			}
			else
			{
				Debug.LogWarning($"ClueId {updated.clue_id}:NotFound");
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			files[0].ActiveInteractable();
			//メソッド
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			files[1].ActiveInteractable();
			//メソッド
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			files[2].ActiveInteractable();
			//メソッド
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			files[3].ActiveInteractable();
			//メソッド
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			files[4].ActiveInteractable();
			//メソッド
		}
	}

	void FileSetup()
	{
		foreach (var file in files)
		{
			file.GetMonitorController(this);
		}
	}

	//引数でサーバーからの値そのまま入れても良さそ

	void PanelSetup()
	{
		for (int i = 0; i < infoPanels.Length; i++)
		{
			//暗号のImageと手がかりのImageを手に入れる
			///i+rangeに変える
			Sprite clueImage = clueImageOutline;
			//Debug.Log(clueImage);
			var codeData = codeController.SetClueCipher();

			infoPanels[i].GetComponent<Com_ClueInfo>().SetPanelImages(clueImage,codeData.Item1, codeData.Item2);
			infoPanels[i].SetActive(false);
		}

		inputManager.GetMonitorController(this);

	}

	//ボタンが押された時に呼ばれるメソッド
    public void DisplayInfoPanel(int num)
    {
		//Debug.Log("num"+num);
        if (panelObj)
        {
            panelObj.SetActive(false);
        }
        panelObj = infoPanels[num];
		//infoPanels[num].GetComponent<Com_ClueInfo>().Imageの設定
		panelObj.SetActive(true);
	}

	//手がかりをエージェントが入手したらモニターコントローラーのメソッドを呼び出し
	//その番号のあまり-1の配列番号のボタンのメソッドを呼びInteractableをTrueにする

	//InputFieldから送られた答えをInfoの中で比較してBool型の戻り値であってたかどうかを
	//確認、あってたらInfoのisSolvedをTrueにしてInputFieldのメッセージにうまくいったことをこいつが
	//伝えるうまくいってなかったらメッセージのみ
	public void SendCodeText(string message)
	{
		//panelObjがNullならDisplayTextをテキストはまだ入力されてないみたいなのにする
		if (!panelObj) 
		{ 
			inputManager.SetDisplayText(0);
			return;
		}
		bool ans = panelObj.GetComponent<Com_ClueInfo>().VerifyAnswer(message);
		if (ans)
		{
			inputManager.SetDisplayText(1);
		}
        else
        {
            inputManager.SetDisplayText(2);
        }
    }

	//void TruthSetup()
	//{
	//	//その時の真相に合わせて使う真相の配列を選ぶ
	//	switch (truth)
	//	{
	//		case AI_RAMPAGE:
	//			truthArray = truth1Array;
	//			break;
	//		case ALIEN_INVASION:
	//			truthArray = truth2Array;
	//			break;
	//		case EXTREME_WEATHER:
	//			truthArray = truth3Array;
	//			break;
	//		case PANDEMIC:
	//			truthArray = truth4Array;
	//			break;
	//		case POLITICAL_TURMOIL:
	//			truthArray = truth5Array;
	//			break;
	//		case DIMENSIONS:
	//			truthArray = truth6Array;
	//			break;
	//	}
	//}
}
