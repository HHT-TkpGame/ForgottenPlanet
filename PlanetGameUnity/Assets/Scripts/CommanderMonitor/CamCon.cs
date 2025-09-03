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

	//�ړ��̃X�N���v�g���ʂȂ̂ł�����ZoomedIn�̎��͈ړ����Ȃ��悤��


	[SerializeField] GameObject camPivot;
    [SerializeField] Camera cam;
    [SerializeField] float sensitivity; // �}�E�X���x
    [SerializeField] float pitchMin;  // �㉺��]�̍ŏ��p�x
    [SerializeField] float pitchMax;   // �㉺��]�̍ő�p�x
    [SerializeField] Transform currentPos;
    float pitch; // ���݂̏㉺�p�x
    
	float rayDistance = 5f;

    const int MAX_MONITORS = 3;
	//monitor���Image�̔z�񂱂�5��UICon�s��?
    [SerializeField] GameObject[] monitor_State_Array=new GameObject[MAX_MONITORS];
	[SerializeField] GraphicRaycaster[] monitor_Graphic_Array = new GraphicRaycaster[MAX_MONITORS];
	[SerializeField] GameObject defaultCanvas;
	[SerializeField] Button leftButton;
	[SerializeField] Button rightButton;


	//�����Commander����
	[SerializeField] Transform[] monitor_Trans_Array = new Transform[MAX_MONITORS];

    PlayerInput input;
    InputAction searchAct;
    InputAction zoomAct;

	const int LEFT = 0;
	const int CENTER = 1;
	const int RIGHT = 2;

	const int VIEW_DEFAULT_RATE = 60;


	/// <summary>
	/// ���ۂɃ��X�g��ύX���Ă݂ă{�^���������邩�ǂ���
	/// 
	/// �肪����̓��e�l����
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

		//�J�����̏����ݒ�̓R�}���_�[��
        cam.fieldOfView = VIEW_DEFAULT_RATE;


		for (int i = 0; i < MAX_MONITORS; i++)
		{
			monitor_State_Array[i].SetActive(false);
			SetGraphicArray(i, false);
		}

		//����UIController�s��
		defaultCanvas.SetActive(false);
	}


	//�X�^�[�e�b�h�̒��͊�{���̂܂�
	void OnSearchStarted(InputAction.CallbackContext context)
    {
		if (state == zoomState.ZoomedIn) { return; }

		Ray ray = new Ray(cam.transform.position, cam.transform.forward);
		if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
		{
			CommanderMonitor monitor = hit.collider.GetComponent<CommanderMonitor>();
			if (monitor != null)
			{
				//����UIController�s��
				//���j�^�[�i�����k������Ȃ��Ȃ炻�̃i���Ԗڂ�SetActiveFalse����
				monitorNum = monitor.Num;

				//����UIController�s��
				//���j�^�[�i���Ԗڂ�True�ɂ���

				//�����炱�ꂢ���
				foreach (var UI in monitor_State_Array)
				{
					///monitor��Num�������Ă��Ă��̔ԍ���UI��\��
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
				//���̕b����0�ȉ��ɂȂ����Ƃ���UI�͏o�������Ǌg��͂��Ȃ������Ƃ�������
				//canZoom��false�ɂ���UI��S�Ĕ�\���ɂ���
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

			//input����̐M����1,2,3�̂ǂꂩ�������͂͂Ȃ��̂�Cast���Ă����v
			int keyNum=int.Parse(keyName);

			//input������͂��ꂽ�l�Ɖ�ʂɏo�Ă���UI����v���Ă��鎞�̂�Zoom
			//�L�[�{�[�h��1�`3������MonitorNum�͔z��ɂ��g������0�`2�ɂȂ��Ă�
			if (keyNum == monitorNum+1)
			{
				Zoom();
			}
			else
			{
				Debug.Log("��v���Ȃ�����");
			}
		}


	}

	private void Zoom()
	{
		currentRotation = cam.transform.localEulerAngles;
		
		//������MonitorNum�������Ƃ���
		cam.transform.position = monitor_Trans_Array[monitorNum].transform.position;
		cam.transform.eulerAngles = monitor_Trans_Array[monitorNum].eulerAngles;
		//�X�e�[�g�̕ύX
		state = zoomState.ZoomedIn;

		//����UIController�s��
		//ChangeButtonState();��MonitorNum�������Ƃ���
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

        // ������]�iY���j: �J�����̐e����]
        camPivot.transform.Rotate(Vector3.up, mouseX, Space.World);

        // ������]�iX���j: �㉺�̊p�x�ɐ�����������
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        cam.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }

    //�{�^������Ăяo�����
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
	//		//�g��
	//		if (cameraView > VIEW_MIN_RATE)
	//		{
	//			cameraView -= Time.deltaTime * RATE;
	//			cam.fieldOfView = cameraView;
	//		}
	//	}
	//	if (Input.GetKey(KeyCode.E))
	//	{
	//		//�g��
	//		if (cameraView < VIEW_MAX_RATE)
	//		{
	//			cameraView += Time.deltaTime * RATE;
	//			cam.fieldOfView = cameraView;
	//		}
	//	}
	//}

}
