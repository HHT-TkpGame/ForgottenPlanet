using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// ScanColを孫にして自分を送るメソッドを孫側で設定
    /// 将来的にはAgent側のみで使えるように
    /// </summary>
    
    [SerializeField, Header("カメラ")] GameObject cameraObj;
    [SerializeField, Header("スキャン用のコライダー")] GameObject scanColObj;


	//分けて別のものにしたほうがいいかも
	//onScanStartedのとこでSetActiveするものとScanでSetActiveするものは
    //扇形が真上で待機する体制になってしまう
	[SerializeField, Header("スキャン用のモデル")] GameObject scanModel;

    //Agent?
	RayTest rayTest;
    BoxCollider scanCol;

    PlayerInput input;

    InputAction moveAct;
    InputAction lookAct;
	InputAction scanAction;

	CharacterController characterController;

    Vector2 moveAxis;
    Vector2 lookAxis;
	
    Vector3 rot;
    Vector3 currentRot;

	const int MOVESPEED = 5;
    const int GRAVITY = 200;

    const int ROTSPEED = 90;

    float verticalVelocity= 0;
    float cameraPitch = 0;

	const float MAX_HOLDTIME = 2f;
	float holdTime = 0f;
	bool actionExecuted = false;
	const int SCAN_ROT_RATIO = -90;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        //if (!MatchingManager.IsCommander) { return; }

        //Update内で動くInputの登録
        input = GetComponent<PlayerInput>();

        moveAct = input.actions["Move"];
        lookAct = input.actions["Look"];

        characterController = GetComponent<CharacterController>();
        currentRot = transform.eulerAngles;

        cameraObj.transform.position = transform.position;
        cameraObj.transform.SetParent(transform);
        cameraObj.transform.eulerAngles = Vector3.zero;

        scanColObj.transform.position = transform.position;
        scanColObj.transform.SetParent(cameraObj.transform);
        scanColObj.transform.eulerAngles=Vector3.zero;

        rayTest = scanColObj.GetComponent<RayTest>();
        rayTest.GetPlayerController(this);

        scanCol = scanColObj.GetComponent<BoxCollider>();

        scanAction = input.actions["Search"];
        scanAction.started += onScanStarted;
        scanAction.canceled += onScanCanceled;

        scanModel.SetActive(false);
        scanCol.enabled = false;
    }

	void onScanStarted(InputAction.CallbackContext context)
	{
		//isScanning = true;
		scanCol.enabled = true;
        scanModel.SetActive(true);
	}
	void onScanCanceled(InputAction.CallbackContext context)
	{
		scanCol.enabled = false;
        scanModel.SetActive(false);
		ResetHold();
	}

	// Update is called once per frame
	void Update()
    {
        //if (!MatchingManager.IsCommander) { return; }
        Move();
        Look();
    }

    void Move()
    {
        moveAxis=moveAct.ReadValue<Vector2>();

        if (!characterController.isGrounded)
        {
            verticalVelocity = -GRAVITY * Time.deltaTime;
        }

		Vector3 moveDir = new Vector3(moveAxis.x * MOVESPEED, verticalVelocity, moveAxis.y * MOVESPEED);
        moveDir=transform.TransformDirection(moveDir);
		characterController.Move(moveDir * Time.deltaTime);
    }

    void Look()
    {

        //if (!MatchingManager.IsCommander) { return; }

        lookAxis = lookAct.ReadValue<Vector2>();



        float rot_Y = lookAxis.x * 0.5f;
        float rot_X = lookAxis.y * 0.5f;

        rot = new Vector3(rot_X * ROTSPEED, rot_Y * ROTSPEED, 0);

        rot *= Time.deltaTime;


        //左右はそのまま上下は逆にして使いたいのでcameraの回転はマイナスを掛ける
        //transform.eulerAngles += new Vector3(0, rot.y, 0);
        transform.Rotate(0, rot.y, 0);

        cameraPitch += rot.x;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        cameraObj.transform.localEulerAngles = new Vector3(-cameraPitch, 0, 0);

    }


    public void Scan(GameObject scanObj)
    {
		holdTime += Time.deltaTime;
        float scanRot=holdTime*SCAN_ROT_RATIO;
        //Debug.Log("scanRot" + scanRot);
		if (holdTime >= MAX_HOLDTIME && !actionExecuted)
		{
			ExecuteAction(scanObj);
			actionExecuted = true;
		}
        if (scanRot > SCAN_ROT_RATIO * 2)
        {
            scanModel.transform.localEulerAngles = new Vector3(scanRot, 180, -45);
        }
	}

	void ExecuteAction(GameObject target)
	{

		//target.GetComponent<ClueBehavior>
		ClueBehavior clue = target.GetComponent<ClueBehavior>();
		//Bool値を見てTrueなら
		if (clue.HasClue)
		{
			//手がかりの番号を手に入れる
			int clueNum = clue.ClueNum;
			//送るなどする

			Debug.Log("手がかりは見つかった番号は" + clueNum + "だ！");
		}
		else
		{
			//falseならDebugする
			Debug.Log("手がかりはなかったようだ....");
		}
		//targetのMeshRendererをEnableする一旦
		target.SetActive(false);
        //そのまま長押しでも別の場所をスキャンできるようにするために
        //初期化
        holdTime = 0;
	}
	public void ResetHold()
	{
		holdTime = 0f;
		actionExecuted = false;
		scanModel.transform.localEulerAngles = new Vector3(0, 180, -45);
		//Debug.Log("外れた");
	}

}
