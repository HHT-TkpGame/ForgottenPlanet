using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour,I_PlayerDefaultFunctions
{
	//サーバーに送るAgentがどこにいるのかを保持するプロパティ
	public Vector3 AgentPos=>gameObject.transform.position;
	public float AgentRot_Y=>gameObject.transform.eulerAngles.y;



	[SerializeField, Header("カメラ")] GameObject cameraObj;

	CharacterController characterController;

	//I_PlayerDefaultFunctions i_function;

	Vector2 moveAxis;
	Vector2 lookAxis;

	Vector3 rot;

	const int MOVESPEED = 5;
	const int GRAVITY = 200;

	const int ROTSPEED = 90;

	float verticalVelocity = 0;

	bool finSetUp;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public void Init()
	{
		characterController = GetComponent<CharacterController>();

		//カメラを自分の子供にする
		cameraObj.transform.SetParent(transform);
		cameraObj.transform.eulerAngles = Vector3.zero;

		//初期位置に配置
		//transform.position = Vector3.zero;

		finSetUp = true;
	}
	public void SetStartPos(Vector3 pos)
	{
		transform.position = pos;
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


		//左右はそのまま上下は逆にして使いたいのでcameraの回転はマイナスを掛ける
		transform.eulerAngles += new Vector3(0, rot.y, 0);
		cameraObj.transform.eulerAngles += new Vector3(-rot.x, 0, 0);

		if (cameraObj.transform.eulerAngles.x > 45 && cameraObj.transform.eulerAngles.x < 360 - 45)
		{
			cameraObj.transform.eulerAngles += new Vector3(rot.x, 0, 0);
		}

	}
}
