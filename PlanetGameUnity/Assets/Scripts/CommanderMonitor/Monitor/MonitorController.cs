using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class MonitorController : MonoBehaviour
{
    const int MAXCLUES = 5;
    [SerializeField, Header("File�̔z��")] FileBehavior[] files=new FileBehavior[MAXCLUES];
    [SerializeField, Header("Info��Panel�̔z��")] GameObject[] infoPanels=new GameObject[MAXCLUES];

 //   [SerializeField, Header("AI�̖\���̔z��")]
 //   Sprite[] truth1Array;
	//[SerializeField, Header("�ِ��l�̔z��")]
	//Sprite[] truth2Array;
	//[SerializeField, Header("�ُ�C�ۂ̔z��")]
	//Sprite[] truth3Array;
	//[SerializeField, Header("�p���f�~�b�N�̔z��")]
	//Sprite[] truth4Array;
	//[SerializeField, Header("�����̔z��")]
	//Sprite[] truth5Array;
	//[SerializeField, Header("�����̔z��")]
	//Sprite[] truth6Array;

	[SerializeField] CodeController codeController;
	[SerializeField] InputFieldManager inputManager;

	//�\������Ă���Panel������z��
	GameObject panelObj;

	CluesManager cluesManager;



	//���̎��̐^���ɍ��킹�Ďg���z��
	Sprite[] truthArray;

	[SerializeField] CluesTextSetter setter;
	[SerializeField] Sprite clueImageOutline;

	public void Init(CluesManager cluesManager)
	{
		this.cluesManager = cluesManager;
		Debug.Log(this.cluesManager.MatchClues);

		codeController.SetCodeList();
		setter.SetText(this.cluesManager.MatchClues.truthId,this.cluesManager.MatchClues.clueIds);
		FileSetup();
		//TruthSetup();
		PanelSetup();

	}

	public void UpdateUI(List<ClueSharedInfo> clues)
	{
		foreach (ClueSharedInfo updated in clues)
		{
			
			// MatchClues.clueIds �̒�����Ώۂ� clue_id �̃C���f�b�N�X��T��
			//�C���f�b�N�X�ɋA���Ă�����̂̓��X�g�̉��Ԗڂɂ��邩
			int index = System.Array.IndexOf(cluesManager.MatchClues.clueIds, updated.clue_id);

			Debug.Log("MatchClues.clueIds"+cluesManager.MatchClues.clueIds[0]);
			Debug.Log("index"+index+"clueId"+updated.clue_id);
			
			//������Ȃ������Ƃ�index��-1�ɂȂ邩����ꂽ�ق��������C������
			//if (index < 0) { return; }

			//UI�X�V
			//�z��̗v�f�ԍ��̓[�����炾����ClueId��1���炾���炻�̗��R����-1
			files[index].ActiveInteractable();
		}
	}

	private void Update()
	{
		//if (Input.GetKeyDown(KeyCode.Q))
		//{
		//	clueGettingStates[3].is_shared=true;
		//	UpdateUI(clueGettingStates);
		//	//���\�b�h
		//}
	}

	void FileSetup()
	{
		foreach (var file in files)
		{
			file.GetMonitorController(this);
		}
	}

	////�����ŃT�[�o�[����̒l���̂܂ܓ���Ă��ǂ���
	//void TruthSetup()
	//{
	//	//���̎��̐^���ɍ��킹�Ďg���^���̔z���I��
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

	void PanelSetup()
	{
		for (int i = 0; i < infoPanels.Length; i++)
		{
			//�Í���Image�Ǝ肪�����Image����ɓ����
			///i+range�ɕς���
			Sprite clueImage = clueImageOutline;

			//Debug.Log(clueImage);
			var codeData = codeController.SetClueCipher();


			infoPanels[i].GetComponent<Com_ClueInfo>().SetPanelImages(clueImage, codeData.Item1, codeData.Item2);
			infoPanels[i].SetActive(false);
		}

		inputManager.GetMonitorController(this);

	}

	//�{�^���������ꂽ���ɌĂ΂�郁�\�b�h
    public void DisplayInfoPanel(int num)
    {
        if (panelObj)
        {
            panelObj.SetActive(false);
        }
        panelObj = infoPanels[num];
		//infoPanels[num].GetComponent<Com_ClueInfo>().Image�̐ݒ�
		panelObj.SetActive(true);
	}

	//�肪������G�[�W�F���g�����肵���烂�j�^�[�R���g���[���[�̃��\�b�h���Ăяo��
	//���̔ԍ��̂��܂�-1�̔z��ԍ��̃{�^���̃��\�b�h���Ă�Interactable��True�ɂ���


	//InputField���瑗��ꂽ������Info�̒��Ŕ�r����Bool�^�̖߂�l�ł����Ă����ǂ�����
	//�m�F�A�����Ă���Info��isSolved��True�ɂ���InputField�̃��b�Z�[�W�ɂ��܂����������Ƃ�������
	//�`���邤�܂������ĂȂ������烁�b�Z�[�W�̂�
	public void SendCodeText(string message)
	{
		//panelObj��Null�Ȃ�DisplayText���e�L�X�g�͂܂����͂���ĂȂ��݂����Ȃ̂ɂ���
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
}
