using System.Collections.Generic;
using UnityEngine;

public class CluesManager : MonoBehaviour
{

	[SerializeField, Header("真相のScriptableObject")] PlanetTruthList planetTruthList;

	CurrentMatchClues currentMatchClues;
    //現在の手がかりの数
    const int MAXCLUES = 5;

	int[] roomNumArray = new int[MAXCLUES];
	int[] idArray;

	List<GameObject> devices = new List<GameObject>();

	void Start()
	{
		Init();
	}

	void Init()
	{
		planetTruthList.LoadCsvData();
		Debug.Log(planetTruthList.DataList[1]);
		//二週目以降リセットするためメソッド化
		int truthId = CluesDataGetter.Instance.Data.truth_id;
		int clueRangeStart = CluesDataGetter.Instance.Data.clues_range[0];
		int clueRangeEnd = CluesDataGetter.Instance.Data.clues_range[1];

		Debug.Log($"id:{truthId}, start:{clueRangeStart}, end:{clueRangeEnd}");

		//サーバーから受け取った値に対応するリストを探す
        PlanetTruth target = planetTruthList.DataList.Find(pt =>
			pt.Truth == truthId &&
			pt.IdNo1 == clueRangeStart 
		);
        if(target != null)
		{
			Debug.Log(target);
			currentMatchClues = new CurrentMatchClues(target.Truth, target.IdNo1, target.IdNo5);
		}
		string s = "";
		for(int i = 0; i < currentMatchClues.clueIds.Length;i++)
		{
			s += currentMatchClues.clueIds[i].ToString() + ", ";
		}
		Debug.Log($"今回の真相ID:{currentMatchClues.truthId}, 手がかりID:{s}");

		GetArrayId(currentMatchClues.clueIds);

		//リストの中に自分の子供の電子機器を入れる
		devices = AddDevices();

		//今回手がかりとする電子機器を決める
		DeliverClue();
	}

	void GetArrayId(int[] clueIds)
	{
		idArray = new int [MAXCLUES];
		for(int i = 0;i < clueIds.Length; i++)
		{
			idArray[i] = clueIds[i];
		}
	}


	List<GameObject> AddDevices()
	{
		List<GameObject> children= new List <GameObject>();
		for(int i = 0; i < transform.childCount;i++)
		{
			children.Add(transform.GetChild(i).gameObject);
		}
		return children;
	}

	void DeliverClue()
	{
		//もし確定で手がかりにしたいものがあるなら
		//forループの前に入れる

		for (int i = 0; i < MAXCLUES; i++)
		{
			ClueData clueData = ChooseDevice();
			//二つ目以降の手がかりの電子機器が同じ部屋から選ばれないようにする配列
			roomNumArray[i] = clueData.clue.RoomNum;
			//選ばれた電子機器に選ばれたことを伝えるメソッド
			clueData.clue.GetClue(idArray[i]);
			//同じ電子機器がもう一度選ばれないようにリストから外す
			devices.Remove(devices[clueData.rnd]);
		}
	}

	ClueData ChooseDevice()
	{
		//抽選をして手がかりとして使うオブジェクトを選ぶメソッド
		//同じ部屋から二つは選ばれてはいけない
		//三つ以上選ばれないようにだともう一個配列作ってrnd番目を+1するとか

		//戻り値として二つの値を使う
		var clueData = new ClueData();
		//抽選を終了してもいいか
		bool canSignUp;

		do
		{
			//一度でもRoomNumが衝突したら無限ループになってしまうのを防ぐ
			canSignUp = true;
			//抽選
			clueData.rnd = Random.Range(0, devices.Count);
			clueData.clue = devices[clueData.rnd].GetComponent<ClueBehavior>();

			//配列は最初から全て0が入っているためRoomNumは1から始める
			foreach (int roomNum in roomNumArray)
			{
				if (roomNum == clueData.clue.RoomNum)
				{
					canSignUp = false;
				}
			}
		} while (!canSignUp);
		return clueData;
	}
}

