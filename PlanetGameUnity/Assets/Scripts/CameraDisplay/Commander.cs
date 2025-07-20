using UnityEngine;
using UnityEngine.InputSystem;

public class Commander : MonoBehaviour,I_PlayerDefaultFunctions
{

	[SerializeField, Header("カメラ")] GameObject cameraObj;
	[SerializeField, Header("初期位置")] Transform startPos; 

	CharacterController characterController;

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

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public void Init()
    {
		characterController = GetComponent<CharacterController>();

		cameraObj.transform.position = startPos.position;

		//カメラを自分の子供にする
		cameraObj.transform.SetParent(transform);
		cameraObj.transform.eulerAngles = Vector3.zero;


		//初期位置に配置
		//transform.position= Vector3.zero;

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

	//


	// Update is called once per frame
	void Update()
    {
        
    }
}
