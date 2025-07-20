using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class CluesManager : MonoBehaviour
{
	class ClueData
	{
		public int rnd;
		public ClueBehavior clue;
	}

	[System.Serializable]
	class Truth
	{
		public int truth;
		public string truthName;
		public Truth(int truth, string truthName)
		{
			this.truth = truth;
			this.truthName = truthName;
		}
	}

	[SerializeField, Header("真相のScriptableObject")] PlanetTruthList planetTruthList;

	//現在の手がかりの数
	const int MAXCLUES = 5;

	int[] roomNumArray = new int[MAXCLUES];
	int[] idArray;

	List<GameObject> devices = new List<GameObject>();

	PlanetTruth planetTruth;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		Init();

	}

	void Init()
	{
		//二週目以降リセットするためメソッド化
		int rnd = Random.Range(0, planetTruthList.DataList.Count);
		planetTruth = planetTruthList.DataList[rnd];

		GetArrayId();

		//相手に送る真相
		Truth truth = new Truth(planetTruth.Truth, planetTruth.TruthName);

		Debug.Log("真相の数値は"+truth.truth+"真相は"+truth.truthName);
		//planetTruthで取ったIdNoXを選ばれたスクリプトに入れる

		//リストの中に自分の子供の電子機器を入れる
		devices = AddDevices();

		//今回手がかりとする電子機器を決める
		DeliverClue();
	}

	void GetArrayId()
	{
		idArray = new int [MAXCLUES]{ planetTruth.IdNo1,planetTruth.IdNo2,
			planetTruth.IdNo3,planetTruth.IdNo4,planetTruth.IdNo5};
	}


	List<GameObject> AddDevices()
	{
		List<GameObject> children= new List <GameObject>();
		for(int i = 0;i<transform.childCount;i++)
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
			var clueData = ChooseDevice();
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

