using System.Collections.Generic;
using UnityEngine;


public class CluesManager : MonoBehaviour
{
	[SerializeField, Header("�^����ScriptableObject")] PlanetTruthList planetTruthList;
	[SerializeField] ClueClientPoller poller;
	//[SerializeField] ���j�^�[�\���p�̃N���X

	public CurrentMatchClues MatchClues {  get; private set; }
    //���݂̎肪����̐�
    const int MAXCLUES = 5;

	int[] roomNumArray = new int[MAXCLUES];
	int[] idArray;

	List<GameObject> devices = new List<GameObject>();

	void Start()
	{
		if (MatchingManager.IsCommander)
		{
			//poller���g���̂�Commander����
			poller.Init(this);
			poller.OnSharedUpdated += UpdateMatchClues;
            //poller.OnSharedUpdated += ���j�^�[�\���p�̃N���X�̕\�����\�b�h �����̌^��<ClueSharedInfo>
            poller.StartLoop();
		}
		Init();
	}
    private void OnDestroy()
    {
        if(MatchingManager.IsCommander)
		{
			poller.OnSharedUpdated -= UpdateMatchClues;
			//poller.OnSharedUpdated -= 
		}
    }

    void Init()
	{
		planetTruthList.LoadCsvData();
		//��T�ڈȍ~���Z�b�g���邽�߃��\�b�h��
		SetLocalClue();

		GetArrayId(MatchClues.clueIds);

		//���X�g�̒��Ɏ����̎q���̓d�q�@�������
		devices = AddDevices();

		//����肪����Ƃ���d�q�@������߂�
		DeliverClue();
	}
	/// <summary>
	/// Poller�̃C�x���g���Ύ��Ɏ󂯂Ƃ������X�g����Ή�����isShared���X�V����
	/// </summary>
	/// <param name="clues"></param>
	void UpdateMatchClues(List<ClueSharedInfo> clues)
	{
        foreach (ClueSharedInfo updated in clues)
        {
            // MatchClues.clueIds �̒�����Ώۂ� clue_id �̃C���f�b�N�X��T��
            int index = System.Array.IndexOf(MatchClues.clueIds, updated.clue_id);

            if (index >= 0)
            {
                MatchClues.isShared[index] = updated.is_shared;
				Debug.Log($"MatchClues:{MatchClues.isShared[0]},{MatchClues.isShared[1]},{MatchClues.isShared[2]},{MatchClues.isShared[3]},{MatchClues.isShared[4]}");
            }
            else
            {
                Debug.LogWarning($"ClueId {updated.clue_id}:NotFound");
            }
        }
    }
	void SetLocalClue()
	{
        int truthId = CluesDataGetter.Instance.Data.truth_id;
        int clueRangeStart = CluesDataGetter.Instance.Data.clues_range[0];
        int clueRangeEnd = CluesDataGetter.Instance.Data.clues_range[1];

        Debug.Log($"id:{truthId}, start:{clueRangeStart}, end:{clueRangeEnd}");

        //�T�[�o�[����󂯎�����l�ɑΉ����郊�X�g��T��
        PlanetTruth target = planetTruthList.DataList.Find(pt =>
            pt.Truth == truthId &&
            pt.IdNo1 == clueRangeStart
        );
        if (target != null)
        {
            MatchClues = new CurrentMatchClues(target.Truth, target.IdNo1, target.IdNo5);
        }
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

    /// <summary>
    /// ����̃��W�b�N�̓s����AClueBehavior��[SerializeField]�ɂ́A
	/// 1�ȏ� && �v5��ވȏ�̒l���K�v�i�Œ�ł�5��ClueBehavior���A�^�b�`���ꂽGameObject���K�v�j
    /// </summary>
    void DeliverClue()
	{
		//�����m��Ŏ肪����ɂ��������̂�����Ȃ�
		//for���[�v�̑O�ɓ����

		for (int i = 0; i < MAXCLUES; i++)
		{
			ClueData clueData = ChooseDevice();
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

			//�z��͍ŏ�����S��0�������Ă��邽��[SerializeFiled]��RoomNum��1����n�߂�
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

