using UnityEngine;

public class Age_ScanAction : MonoBehaviour, I_SearchAction
{
	//Agent�̒��g���ڍs

	[SerializeField, Header("�X�L�����p�̃R���C�_�[")] GameObject scanColObj;
	////�����ĕʂ̂��̂ɂ����ق�����������
	////onScanStarted�̂Ƃ���SetActive������̂�Scan��SetActive������̂�
	////��`���^��őҋ@����̐��ɂȂ��Ă��܂�
	[SerializeField, Header("�X�L�����p�̃��f��")] GameObject scanModel;
	[SerializeField, Header("���j�^�[��ʎ��̃Q�[��UI")] GameObject gameUIPanel;
	[SerializeField, Header("�G�[�W�F���g�A�җpUI")] GameObject agentReturnUIPanel;
	[SerializeField, Header("�\�����Ȃ��K�C�hUI")] GameObject com_GuideUI;

	ScanColliderBehavior scanColliderBehavior;
	BoxCollider scanBoxCollider;

	const float MAX_HOLDTIME = 2f;
	float holdTime = 0f;
	bool actionExecuted = false;
	const int SCAN_ROT_RATIO = -90;

	public void SetUp(Transform camera)
	{
		scanColObj.transform.SetParent(camera);
		scanColObj.transform.localPosition = Vector3.zero;
		scanColObj.transform.eulerAngles = Vector3.zero;
		scanModel.transform.SetParent(camera);
		scanModel.transform.localPosition = new Vector3(0, 0, 0.2f);

		scanColliderBehavior = scanColObj.GetComponent<ScanColliderBehavior>();
		scanColliderBehavior.GetScanAction(this);

		scanBoxCollider = scanColObj.GetComponent<BoxCollider>();

		scanBoxCollider.enabled = false;

		ResetHold();

		scanModel.SetActive(false);
		gameUIPanel.SetActive(false);
		agentReturnUIPanel.SetActive(false);
		com_GuideUI.SetActive(false);
	}

	public void OnSearchStarted()
	{
		scanBoxCollider.enabled = true;
		scanModel.SetActive(true);
	}

	public void OnSearchCanceled()
	{
		scanBoxCollider.enabled = false;
		scanModel.SetActive(false);
		ResetHold();
	}

	public void OnZoomPerformed(string keyName)
	{

	}

	/// <summary>
	/// �T�[�`�A�N�V�������n�܂�����Collider���o��
	/// Collider��OnTriggerStay����Scan���\�b�h���Ă�
	/// ExecuteAction�ł���Object��Clue�������Ă���Ȃ�l���擾
	/// </summary>
	/// <param name="scanObj"></param>
	public void Scan(GameObject scanObj)
	{
		holdTime += Time.deltaTime;
		float scanRot = holdTime * SCAN_ROT_RATIO + 45;
		//��莞��Scan���\�b�h���Ăяo���ꂽ��
		//actionExecuted�͂��̌シ��������񉺂̃��\�b�h���ĂԂ̖h�~
		if (holdTime >= MAX_HOLDTIME && !actionExecuted)
		{
			ExecuteAction(scanObj);
			actionExecuted = true;
		}
		if (scanRot > SCAN_ROT_RATIO * 2)
		{
			scanModel.transform.localEulerAngles = new Vector3(-scanRot, 0, 0);
		}
	}

	void ExecuteAction(GameObject target)
	{

		//target.GetComponent<ClueBehavior>
		ClueBehavior clue = target.GetComponent<ClueBehavior>();
		//Bool�l������True�Ȃ�
		if (clue.HasClue)
		{
			//�肪����̔ԍ�����ɓ����
			int clueNum = clue.ClueNum;
			//����Ȃǂ���
			StartCoroutine(CluesDataGetter.Instance.ClueClient.PostClue(
				clueNum,
				onSuccess: () =>
				{
					Debug.Log($"Success: �肪����ԍ�{clueNum}");
				},
				onError: (err) =>
				{
                    Debug.Log($"Failure: �肪����ԍ�{clueNum}");
                })
			);
		}
		else
		{
			//false�Ȃ�Debug����
			Debug.Log("�肪����͂Ȃ������悤��....");
		}
		//target��MeshRenderer��Enable�����U
		target.SetActive(false);
	}
	public void ResetHold()
	{
		//�X�L�����̃{�^���������Ă��邪�肪����ɓ������Ă��Ȃ����̏���

		holdTime = 0f;
		actionExecuted = false;

		//�G�t�F�N�g�ȂǂɂȂ����Ƃ��͂���p�̃G�t�F�N�g���Đ�����Ȃ�
		scanModel.transform.localEulerAngles = new Vector3(-45f, 0, 0);
	}
}