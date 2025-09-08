using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour,I_PlayerDefaultFunctions, ITransformProvider
{
	//�T�[�o�[�ɑ���Agent���ǂ��ɂ���̂���ێ�����v���p�e�B
	public Vector3 AgentPos=>transform.position;
	public float AgentRotY=>gameObject.transform.eulerAngles.y;

	[SerializeField, Header("�J����")] GameObject cameraObj;
	[SerializeField, Header("�����ʒu")] Transform startPos;

	//[SerializeField, Header("Scan�֌W��Class")] Age_ScanAction scanAct;

	Age_ScanAction scanAct;

	CharacterController characterController;

	//I_PlayerDefaultFunctions i_function;

	Vector3 rot;

	const int MOVESPEED = 5;
	const int GRAVITY = 200;

	const int ROTSPEED = 50;

	float verticalVelocity = 0;
	float cameraPitch = 0;

	bool finSetUp;

	[SerializeField] AgentWalkSe agentWalk;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public void Init()
	{
		characterController = GetComponent<CharacterController>();

		scanAct=GetComponent<Age_ScanAction>();

		//�J�����������̎q���ɂ���
		cameraObj.transform.SetParent(transform);
        cameraObj.transform.localPosition = new Vector3(0, 0.5f, 0);
		cameraObj.transform.localEulerAngles = Vector3.zero;

		scanAct.SetUp(cameraObj.transform);

		//�����ʒu�ɔz�u
		transform.position = Vector3.zero;

		finSetUp = true;

	}
	public void SetStartPos()
	{
		transform.position = startPos.position;
	}

	public void Move(Vector2 moveAxis)
	{
		if (!finSetUp) { return; }


		if (!characterController.isGrounded)
		{
			verticalVelocity = -GRAVITY * Time.deltaTime;
		}

		if ((moveAxis.x != 0 || moveAxis.y != 0))
		{
			agentWalk.SeStart();
		}

		Vector3 moveDir = new Vector3(moveAxis.x * MOVESPEED, verticalVelocity, moveAxis.y * MOVESPEED);
		moveDir = transform.TransformDirection(moveDir);
		characterController.Move(moveDir * Time.deltaTime);
	}

	public void Look(Vector2 lookAxis)
	{
		if (!finSetUp) { return; }

		//if (!MatchingManager.IsCommander) { return; }


		float rot_Y = lookAxis.x;
		float rot_X = lookAxis.y;

		rot = new Vector3(rot_X * ROTSPEED, rot_Y * ROTSPEED, 0);

		rot *= Time.deltaTime;

		transform.Rotate(0, rot.y, 0);

		cameraPitch += rot.x;
		cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
		cameraObj.transform.localEulerAngles = new Vector3(-cameraPitch, 0, 0);
	}
}
