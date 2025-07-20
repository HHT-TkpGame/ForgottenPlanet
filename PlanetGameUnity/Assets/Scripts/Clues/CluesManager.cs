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

	[SerializeField, Header("�^����ScriptableObject")] PlanetTruthList planetTruthList;

	//���݂̎肪����̐�
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
		//��T�ڈȍ~���Z�b�g���邽�߃��\�b�h��
		int rnd = Random.Range(0, planetTruthList.DataList.Count);
		planetTruth = planetTruthList.DataList[rnd];

		GetArrayId();

		//����ɑ���^��
		Truth truth = new Truth(planetTruth.Truth, planetTruth.TruthName);

		Debug.Log("�^���̐��l��"+truth.truth+"�^����"+truth.truthName);
		//planetTruth�Ŏ����IdNoX��I�΂ꂽ�X�N���v�g�ɓ����

		//���X�g�̒��Ɏ����̎q���̓d�q�@�������
		devices = AddDevices();

		//����肪����Ƃ���d�q�@������߂�
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
		//�����m��Ŏ肪����ɂ��������̂�����Ȃ�
		//for���[�v�̑O�ɓ����

		for (int i = 0; i < MAXCLUES; i++)
		{
			var clueData = ChooseDevice();
			//��ڈȍ~�̎肪����̓d�q�@�킪������������I�΂�Ȃ��悤�ɂ���z��
			roomNumArray[i] = clueData.clue.RoomNum;
			//�I�΂ꂽ�d�q�@��ɑI�΂ꂽ���Ƃ�`���郁�\�b�h
			clueData.clue.GetClue(idArray[i]);
			//�����d�q�@�킪������x�I�΂�Ȃ��悤�Ƀ��X�g����O��
			devices.Remove(devices[clueData.rnd]);
		}
	}

	ClueData ChooseDevice()
	{
		//���I�����Ď肪����Ƃ��Ďg���I�u�W�F�N�g��I�ԃ��\�b�h
		//�������������͑I�΂�Ă͂����Ȃ�
		//�O�ȏ�I�΂�Ȃ��悤�ɂ��Ƃ�����z������rnd�Ԗڂ�+1����Ƃ�

		//�߂�l�Ƃ��ē�̒l���g��
		var clueData = new ClueData();
		//���I���I�����Ă�������
		bool canSignUp;

		do
		{
			//��x�ł�RoomNum���Փ˂����疳�����[�v�ɂȂ��Ă��܂��̂�h��
			canSignUp = true;
			//���I
			clueData.rnd = Random.Range(0, devices.Count);
			clueData.clue = devices[clueData.rnd].GetComponent<ClueBehavior>();

			//�z��͍ŏ�����S��0�������Ă��邽��RoomNum��1����n�߂�
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

