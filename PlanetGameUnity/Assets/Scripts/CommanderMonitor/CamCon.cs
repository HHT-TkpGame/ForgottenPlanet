using System.Security;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class CamCon : MonoBehaviour
{

    public enum zoomState
    {
       Default,
       Select,
       ZoomedIn
    }

    zoomState state = zoomState.Default;
	public zoomState State => state;

	//移動のスクリプトが別なのでそこでZoomedInの時は移動しないように


	[SerializeField] GameObject camPivot;
    [SerializeField] Camera cam;
    [SerializeField] float sensitivity; // マウス感度
    [SerializeField] float pitchMin;  // 上下回転の最小角度
    [SerializeField] float pitchMax;   // 上下回転の最大角度
    [SerializeField] Transform currentPos;
    float pitch; // 現在の上下角度
    
	float rayDistance = 5f;

    const int MAX_MONITORS = 3;
	//monitor上のImageの配列これ5つはUICon行き?
    [SerializeField] GameObject[] monitor_State_Array=new GameObject[MAX_MONITORS];
	[SerializeField] GraphicRaycaster[] monitor_Graphic_Array = new GraphicRaycaster[MAX_MONITORS];
	[SerializeField] GameObject defaultCanvas;
	[SerializeField] Button leftButton;
	[SerializeField] Button rightButton;


	//これはCommanderいき
	[SerializeField] Transform[] monitor_Trans_Array = new Transform[MAX_MONITORS];

    PlayerInput input;
    InputAction searchAct;
    InputAction zoomAct;

	const int LEFT = 0;
	const int CENTER = 1;
	const int RIGHT = 2;

	const int VIEW_DEFAULT_RATE = 60;


	/// <summary>
	/// 実際にリストを変更してみてボタンを押せるかどうか
	/// 
	/// 手がかりの内容考える
	/// 
	/// </summary>

	private void Start()
	{
        input = GetComponent<PlayerInput>();

        searchAct = input.actions["Search"];
		searchAct.started += OnSearchStarted;
		searchAct.canceled += OnSearchCanceled;

		//Manager
		zoomAct = input.actions["Com_Zoom"];
		zoomAct.performed += OnZoomPerformed;





        gameObject.transform.position = currentPos.position;

		//カメラの初期設定はコマンダーで
        cam.fieldOfView = VIEW_DEFAULT_RATE;


		for (int i = 0; i < MAX_MONITORS; i++)
		{
			monitor_State_Array[i].SetActive(false);
			SetGraphicArray(i, false);
		}

		//これUIController行き
		defaultCanvas.SetActive(false);
	}


	//スターテッドの中は基本このまま
	void OnSearchStarted(InputAction.CallbackContext context)
    {
		if (state == zoomState.ZoomedIn) { return; }

		Ray ray = new Ray(cam.transform.position, cam.transform.forward);
		if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
		{
			CommanderMonitor monitor = hit.collider.GetComponent<CommanderMonitor>();
			if (monitor != null)
			{
				//これUIController行き
				//モニターナムがヌルじゃないならそのナム番目をSetActiveFalseして
				monitorNum = monitor.Num;

				//これUIController行き
				//モニターナム番目をTrueにする

				//したらこれいらん
				foreach (var UI in monitor_State_Array)
				{
					///monitorのNumを持ってきてその番号のUIを表示
					if (UI == monitor_State_Array[monitorNum])
					{
						UI.SetActive(true);
					}
					else
					{
						UI.SetActive(false);
					}
				}

					state = zoomState.Select;
				//この秒数が0以下になったときはUIは出したけど拡大はしなかったということ
				//canZoomをfalseにしてUIを全て非表示にする
				canZoom_Time = MAX_CANZOOM_TIME;
			}
		}

	}
	void OnSearchCanceled(InputAction.CallbackContext context)
	{

	}
	void OnZoomPerformed(InputAction.CallbackContext context)
	{
		if (state != zoomState.Select) { return; }

		var control=context.control;

		if(control is KeyControl keyControl)
		{
			string keyName = keyControl.name;
			Debug.Log("keyName"+keyName);
			Debug.Log("monitorNum" + monitorNum);

			//inputからの信号は1,2,3のどれかしか入力はないのでCastしても大丈夫
			int keyNum=int.Parse(keyName);

			//inputから入力された値と画面に出ているUIが一致している時のみZoom
			//キーボードは1～3だけどMonitorNumは配列にも使うから0～2になってる
			if (keyNum == monitorNum+1)
			{
				Zoom();
			}
			else
			{
				Debug.Log("一致しなかった");
			}
		}


	}

	private void Zoom()
	{
		currentRotation = cam.transform.localEulerAngles;
		
		//ここもMonitorNumを引数として
		cam.transform.position = monitor_Trans_Array[monitorNum].transform.position;
		cam.transform.eulerAngles = monitor_Trans_Array[monitorNum].eulerAngles;
		//ステートの変更
		state = zoomState.ZoomedIn;

		//これUIController行き
		//ChangeButtonState();はMonitorNumを引数として
		defaultCanvas.SetActive(true);
		ChangeButtonState();

		foreach (var UI in monitor_State_Array)
		{
			UI.SetActive(false);
		}
	}

	Vector3 currentRotation = Vector3.zero;
    int monitorNum;

    const float MAX_CANZOOM_TIME = 5;
    float canZoom_Time;

	void Update()
    {
        Look();

		if(state != zoomState.Select) { return; }
		
        if (canZoom_Time>0)
        {
            canZoom_Time-=Time.deltaTime;
            if (canZoom_Time < 0)
            {
				state = zoomState.Default;

                foreach(var UI in monitor_State_Array)
                {
                    UI.SetActive(false);
                }
            }
        }
    }


    void ChangeButtonState()
    {
		switch (monitorNum)
		{
			case LEFT:
				{
					leftButton.interactable = false;
					rightButton.interactable = true;
					SetGraphicArray(LEFT, true);
					break;
				}
			case CENTER:
				{
					rightButton.interactable = true;
					leftButton.interactable = true;
					SetGraphicArray(CENTER, true);
					break;
				}
			case RIGHT:
				{
					leftButton.interactable = true;
					rightButton.interactable = false;
					SetGraphicArray(RIGHT, true);
					break;
				}
		}
	}

	void SetGraphicArray(int num,bool isActive)
	{
		monitor_Graphic_Array[num].enabled = isActive;
	}



    void Look()
    {
		if (state == zoomState.ZoomedIn) { return; }

		float mouseX = Input.GetAxis("Horizontal") * sensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Vertical") * sensitivity * Time.deltaTime;

		//float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // 水平回転（Y軸）: カメラの親を回転
        camPivot.transform.Rotate(Vector3.up, mouseX, Space.World);

        // 垂直回転（X軸）: 上下の角度に制限をかける
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        cam.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }

    //ボタンから呼び出される
    public void PushLeftButton()
    {
		SetGraphicArray(monitorNum,false);
		monitorNum--;
		cam.transform.position = monitor_Trans_Array[monitorNum].transform.position;
		cam.transform.eulerAngles = monitor_Trans_Array[monitorNum].eulerAngles;
        ChangeButtonState();
	}
	public void PushRightButton()
	{
		SetGraphicArray(monitorNum,false);
        monitorNum++;
		cam.transform.position = monitor_Trans_Array[monitorNum].transform.position;
		cam.transform.eulerAngles = monitor_Trans_Array[monitorNum].eulerAngles;
        ChangeButtonState();
	}
    public void PushCancelButton()
    {
		state = zoomState.Default;
		cam.gameObject.transform.localPosition = Vector3.zero;
        cam.transform.localEulerAngles = currentRotation;
		defaultCanvas.SetActive(false);
	}

	//void Zoom()
	//{
	//	if (Input.GetKey(KeyCode.Q))
	//	{
	//		//拡大
	//		if (cameraView > VIEW_MIN_RATE)
	//		{
	//			cameraView -= Time.deltaTime * RATE;
	//			cam.fieldOfView = cameraView;
	//		}
	//	}
	//	if (Input.GetKey(KeyCode.E))
	//	{
	//		//拡小
	//		if (cameraView < VIEW_MAX_RATE)
	//		{
	//			cameraView += Time.deltaTime * RATE;
	//			cam.fieldOfView = cameraView;
	//		}
	//	}
	//}

}
