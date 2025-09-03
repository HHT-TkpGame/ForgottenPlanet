using UnityEngine;
using UnityEngine.InputSystem;

public class Commander : MonoBehaviour,I_PlayerDefaultFunctions
{
	public enum zoomState
	{
		Default,
		Select,
		ZoomedIn
	}

	zoomState state = zoomState.Default;
	public zoomState State => state;

	const int MAX_MONITORS = 3;

	[SerializeField, Header("カメラ")] GameObject cameraObj;
	[SerializeField, Header("初期位置")] Transform startPos;

	//カメラを移動させる先の座標
	[SerializeField] Transform[] monitor_Trans_Array = new Transform[MAX_MONITORS];

	CharacterController characterController;

	Camera cam;
	public Camera Cam => cam;

	Com_ZoomAction zoomAct;

	//I_PlayerDefaultFunctions i_function;

	Vector2 moveAxis;
	Vector2 lookAxis;

	Vector3 rot;
	Vector3 prevAngle;

	const int MOVESPEED = 5;
	const int GRAVITY = 200;

	const int ROTSPEED = 90;

	const int VIEW_DEFAULT_RATE = 60;

	float verticalVelocity = 0;
	float cameraPitch = 0;

	bool finSetUp;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public void Init()
    {
		characterController = GetComponent<CharacterController>();

		cameraObj.transform.position = startPos.position;

		//カメラを自分の子供にする
		cameraObj.transform.SetParent(transform);
		cameraObj.transform.eulerAngles = Vector3.zero;

		cam = cameraObj.GetComponent<Camera>();
		cam.fieldOfView = VIEW_DEFAULT_RATE;
		//初期位置に配置
		//transform.position= Vector3.zero;

		zoomAct = GetComponent<Com_ZoomAction>();
		zoomAct.SetUp(this);

		finSetUp = true;
	}

	public void SetStartPos()
	{
		transform.position = startPos.position;
	}

	public void SetZoomState(zoomState state)
	{
		this.state=state;
	}
	
	public void ZoomStart(int monitorNum)
	{
		prevAngle=cameraObj.transform.localEulerAngles;
		SetCameraTrans(monitorNum);
		SetZoomState(zoomState.ZoomedIn);
	}

	public void ZoomCancel()
	{
		cam.gameObject.transform.localPosition = Vector3.zero;
		cam.transform.localEulerAngles = prevAngle;
	}

	public void SetCameraTrans(int num)
	{
		cameraObj.transform.position = monitor_Trans_Array[num].transform.position;
		cameraObj.transform.eulerAngles = monitor_Trans_Array[num].eulerAngles;
	}

	public void Move(Vector2 moveAxis)
	{
		if (!finSetUp) { return; }
		if (state == zoomState.ZoomedIn) { return; }

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
		if (state == zoomState.ZoomedIn) { return; }

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
}
