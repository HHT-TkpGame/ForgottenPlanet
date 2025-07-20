using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour,I_PlayerDefaultFunctions, ITransformProvider
{
	//�T�[�o�[�ɑ���Agent���ǂ��ɂ���̂���ێ�����v���p�e�B
	public Vector3 AgentPos=>transform.position;
	public float AgentRotY=>gameObject.transform.eulerAngles.y;

	[SerializeField, Header("�J����")] GameObject cameraObj;
	[SerializeField, Header("�����ʒu")] Transform startPos;

	[SerializeField, Header("�X�L�����p�̃R���C�_�[")] GameObject scanColObj;
	////�����ĕʂ̂��̂ɂ����ق�����������
	////onScanStarted�̂Ƃ���SetActive������̂�Scan��SetActive������̂�
	////��`���^��őҋ@����̐��ɂȂ��Ă��܂�
	//[SerializeField, Header("�X�L�����p�̃��f��")] GameObject scanModel;

	CharacterController characterController;

	ScanColliderBehavior scanColliderBehavior;
	BoxCollider scanBoxCollider;

	//I_PlayerDefaultFunctions i_function;

	Vector2 moveAxis;
	Vector2 lookAxis;

	Vector3 rot;

	const int MOVESPEED = 5;
	const int GRAVITY = 200;

	const int ROTSPEED = 90;

	float verticalVelocity = 0;
	float cameraPitch = 0;

	bool finSetUp;

	//const float MAX_HOLDTIME = 2f;
	//float holdTime = 0f;
	//bool actionExecuted = false;
	//const int SCAN_ROT_RATIO = -90;




	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public void Init()
	{
		characterController = GetComponent<CharacterController>();

		//�J�����������̎q���ɂ���
		cameraObj.transform.SetParent(transform);
        cameraObj.transform.localPosition = Vector3.zero;
        cameraObj.transform.localEulerAngles = Vector3.zero;

		//scanColObj.transform.SetParent(cameraObj.transform);
		//scanColObj.transform.localPosition = Vector3.zero;
		//scanColObj.transform.eulerAngles = Vector3.zero;

		//�����ʒu�ɔz�u
		//transform.position = Vector3.zero;

		//scanColliderBehavior =scanColObj.GetComponent<ScanColliderBehavior>();
		//scanColliderBehavior.GetPlayerController(this);

		//scanBoxCollider=scanColObj.GetComponent<BoxCollider>();

		//scanBoxCollider.enabled = false;

		//scanModel.SetActive(false);

		finSetUp = true;
	}
	public void SetStartPos()
	{
		transform.position = startPos.position;
	}

	public void Move(Vector2 moveAxis)
	{
		if (!finSetUp) { return; }

		this.moveAxis = moveAxis;

		if (!characterController.isGrounded)
		{
			verticalVelocity = -GRAVITY * Time.deltaTime;
		}

		Vector3 moveDir = new Vector3(moveAxis.x * MOVESPEED, verticalVelocity, moveAxis.y * MOVESPEED);
		moveDir = transform.TransformDirection(moveDir);
		characterController.Move(moveDir * Time.deltaTime);
	}

	public void Look(Vector2 lookAxis)
	{
		if (!finSetUp) { return; }
		//Debug.Log(cameraObj.transform.eulerAngles);
		//if (!MatchingManager.IsCommander) { return; }

		this.lookAxis = lookAxis;

		float rot_Y = lookAxis.x;
		float rot_X = lookAxis.y;

		rot = new Vector3(rot_X * ROTSPEED, rot_Y * ROTSPEED, 0);

		rot *= Time.deltaTime;

		transform.Rotate(0, rot.y, 0);

		cameraPitch += rot.x;
		cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
		cameraObj.transform.localEulerAngles = new Vector3(-cameraPitch, 0, 0);
	}


	//public void OnScanStarted()
	//{
	//	scanBoxCollider.enabled = true;
	//	scanModel.SetActive(true);
	//}

	//public void OnScanCanceled()
	//{
	//	scanBoxCollider.enabled = false;
	//	scanModel.SetActive(false);
	//ResetHold();
	//}

	//public void Scan(GameObject scanObj)
	//{
	//	holdTime += Time.deltaTime;
	//	float scanRot = holdTime * SCAN_ROT_RATIO;
	//	Debug.Log("scanRot" + scanRot);
	//	if (holdTime >= MAX_HOLDTIME && !actionExecuted)
	//	{
	//		ExecuteAction(scanObj);
	//		actionExecuted = true;
	//	}
	//	if (scanRot > SCAN_ROT_RATIO * 2)
	//	{
	//		scanModel.transform.localEulerAngles = new Vector3(scanRot, 180, -45);
	//	}
	//}


	//void ExecuteAction(GameObject target)
	//{

	//	//target.GetComponent<ClueBehavior>
	//	ClueBehavior clue = target.GetComponent<ClueBehavior>();
	//	//Bool�l������True�Ȃ�
	//	if (clue.HasClue)
	//	{
	//		//�肪����̔ԍ�����ɓ����
	//		int clueNum = clue.ClueNum;
	//		//����Ȃǂ���

	//		Debug.Log("�肪����͌��������ԍ���" + clueNum + "���I");
	//	}
	//	else
	//	{
	//		//false�Ȃ�Debug����
	//		Debug.Log("�肪����͂Ȃ������悤��....");
	//	}
	//	//target��MeshRenderer��Enable�����U
	//	target.SetActive(false);
	//}

	//public void ResetHold()
	//{
	//	holdTime = 0f;
	//	actionExecuted = false;
	//	//Debug.Log("�O�ꂽ");
	//}
}
