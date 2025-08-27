using UnityEngine;
using UnityEngine.UI;

public class MonitorController : MonoBehaviour
{
    const int MAXCLUES = 5;
    [SerializeField, Header("File�̔z��")] FileBehavior[] files=new FileBehavior[MAXCLUES];
    [SerializeField, Header("Info��Panel�̔z��")] GameObject[] infoPanels=new GameObject[MAXCLUES];

    [SerializeField, Header("AI�̖\���̔z��")]
    Sprite[] truth1Array;
	[SerializeField, Header("�ِ��l�̔z��")]
	Sprite[] truth2Array;
	[SerializeField, Header("�ُ�C�ۂ̔z��")]
	Sprite[] truth3Array;
	[SerializeField, Header("�p���f�~�b�N�̔z��")]
	Sprite[] truth4Array;
	[SerializeField, Header("�����̔z��")]
	Sprite[] truth5Array;
	[SerializeField, Header("�����̔z��")]
	Sprite[] truth6Array;

	[SerializeField] CodeController codeController;
	[SerializeField] InputFieldManager inputManager;

	//�\������Ă���Panel������z��
	GameObject panelObj;

	

	/// <summary>
	/// ��ŃT�[�o�[����̒l��
	/// range�ɂ���Ƃ���-1�����Ƃ�����n�߂�
	/// ������MAXCLUES�̂��܂�ŋ��߂��Ǝv��
	/// </summary>
	int truth;
    int range;

    const int AI_RAMPAGE = 0;
	const int ALIEN_INVASION = 1;
	const int EXTREME_WEATHER= 2;
	const int PANDEMIC = 3;
	const int POLITICAL_TURMOIL = 4;
	const int DIMENSIONS = 5;

	//���̎��̐^���ɍ��킹�Ďg���z��
	Sprite[] truthArray;

	//manager��Init�Ƃ��ɂ��Ă���������

	void Start()
    {
		truth = 0;
		FileSetup();
		TruthSetup();
		PanelSetup();
    }

	void FileSetup()
	{
		foreach (var file in files)
		{
			file.GetMonitorController(this);
		}
	}

	//�����ŃT�[�o�[����̒l���̂܂ܓ���Ă��ǂ���
	void TruthSetup()
	{
		//���̎��̐^���ɍ��킹�Ďg���^���̔z���I��
		switch (truth)
		{
			case AI_RAMPAGE:
				truthArray = truth1Array;
				break;
			case ALIEN_INVASION:
				truthArray = truth2Array;
				break;
			case EXTREME_WEATHER:
				truthArray = truth3Array;
				break;
			case PANDEMIC:
				truthArray = truth4Array;
				break;
			case POLITICAL_TURMOIL:
				truthArray = truth5Array;
				break;
			case DIMENSIONS:
				truthArray = truth6Array;
				break;
		}
	}

	void PanelSetup()
	{
		for (int i = 0; i < infoPanels.Length; i++)
		{
			//�Í���Image�Ǝ肪�����Image����ɓ����
			Sprite clueImage = truthArray[i];
			//Debug.Log(clueImage);
			var codeData = codeController.SetClueCipher(i);


			infoPanels[i].GetComponent<Com_ClueInfo>().SetPanelImages(clueImage, codeData.Item1, codeData.Item2);
			infoPanels[i].SetActive(false);
		}

		inputManager.GetMonitorController(this);
	}

	//�{�^���������ꂽ���ɌĂ΂�郁�\�b�h
    public void DisplayInfoPanel(int num)
    {
		Debug.Log("num"+num);
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
