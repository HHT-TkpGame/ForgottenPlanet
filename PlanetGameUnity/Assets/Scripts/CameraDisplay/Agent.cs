using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour,I_PlayerDefaultFunctions, ITransformProvider
{
	//サーバーに送るAgentがどこにいるのかを保持するプロパティ
	public Vector3 AgentPos=>transform.position;
	public float AgentRotY=>gameObject.transform.eulerAngles.y;

	[SerializeField, Header("カメラ")] GameObject cameraObj;
	[SerializeField, Header("初期位置")] Transform startPos;

	[SerializeField, Header("スキャン用のコライダー")] GameObject scanColObj;
	////分けて別のものにしたほうがいいかも
	////onScanStartedのとこでSetActiveするものとScanでSetActiveするものは
	////扇形が真上で待機する体制になってしまう
	//[SerializeField, Header("スキャン用のモデル")] GameObject scanModel;

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

		//カメラを自分の子供にする
		cameraObj.transform.SetParent(transform);
        cameraObj.transform.localPosition = Vector3.zero;
        cameraObj.transform.localEulerAngles = Vector3.zero;

		//scanColObj.transform.SetParent(cameraObj.transform);
		//scanColObj.transform.localPosition = Vector3.zero;
		//scanColObj.transform.eulerAngles = Vector3.zero;

		//初期位置に配置
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
	//	//Bool値を見てTrueなら
	//	if (clue.HasClue)
	//	{
	//		//手がかりの番号を手に入れる
	//		int clueNum = clue.ClueNum;
	//		//送るなどする

	//		Debug.Log("手がかりは見つかった番号は" + clueNum + "だ！");
	//	}
	//	else
	//	{
	//		//falseならDebugする
	//		Debug.Log("手がかりはなかったようだ....");
	//	}
	//	//targetのMeshRendererをEnableする一旦
	//	target.SetActive(false);
	//}

	//public void ResetHold()
	//{
	//	holdTime = 0f;
	//	actionExecuted = false;
	//	//Debug.Log("外れた");
	//}
}
