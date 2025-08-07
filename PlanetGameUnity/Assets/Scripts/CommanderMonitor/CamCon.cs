using UnityEngine;
using UnityEngine.UI;

public class CamCon : MonoBehaviour
{
    [SerializeField] GameObject camPivot;
    [SerializeField] Camera cam;
    [SerializeField] float sensitivity; // �}�E�X���x
    [SerializeField] float pitchMin;  // �㉺��]�̍ŏ��p�x
    [SerializeField] float pitchMax;   // �㉺��]�̍ő�p�x
    [SerializeField] Transform currentPos;
    float pitch; // ���݂̏㉺�p�x
    float rayDistance = 5f;
    float cameraView;

    const int MAX_MONITORS = 3;
    [SerializeField]GameObject[] monitor_State_Array=new GameObject[MAX_MONITORS];
	[SerializeField] Transform[] monitor_Trans_Array = new Transform[MAX_MONITORS];

    [SerializeField] GameObject defaultCanvas;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;

	private void Start()
	{
        gameObject.transform.position = currentPos.position;
        cam.fieldOfView = VIEW_DEFAULT_RATE;
        cameraView = cam.fieldOfView;
        Debug.Log(transform.position);

        foreach (var UI in monitor_State_Array)
        {
            UI.SetActive(false);
        }
        defaultCanvas.SetActive(false);
	}

    Vector3 currentRotation = Vector3.zero;
    int monitorNum;
    bool canZoom;
    bool isZoom;
    KeyCode numCode;

    const float MAX_CANZOOM_TIME = 5;
    float canZoom_Time;

	void Update()
    {
        Look();

        //Zoom();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            {
                CommanderMonitor monitor = hit.collider.GetComponent<CommanderMonitor>();
                if (monitor != null)
                {
                    monitorNum = monitor.Num;

                    numCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), ("Alpha") + ((monitorNum + 1).ToString()));

                    //Vector3 monitorPos = hit.collider.gameObject.transform.position;

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

                    canZoom = true;
                    //���̕b����0�ȉ��ɂȂ����Ƃ���UI�͏o�������Ǌg��͂��Ȃ������Ƃ�������
                    //canZoom��false�ɂ���UI��S�Ĕ�\���ɂ���
                    canZoom_Time = MAX_CANZOOM_TIME;
                }
            }
        }

        if (canZoom && Input.GetKeyDown(numCode))
        {
            currentRotation = cam.transform.localEulerAngles;
            //Debug.Log("sedad");
            cam.transform.position = monitor_Trans_Array[monitorNum].transform.position;
            cam.transform.eulerAngles = monitor_Trans_Array[monitorNum].eulerAngles;
            isZoom = true;
            defaultCanvas.SetActive(true);
            ChangeButtonState();
			foreach (var UI in monitor_State_Array)
			{
				UI.SetActive(false);
			}
		}

        if (canZoom && !isZoom)
        {
            canZoom_Time-=Time.deltaTime;
            if (canZoom_Time < 0)
            {
                canZoom = false;
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
			case 0:
				{
					leftButton.interactable = false;
					rightButton.interactable = true;
					break;
				}
			case 1:
				{
					rightButton.interactable = true;
					leftButton.interactable = true;
					break;
				}
			case 2:
				{
					leftButton.interactable = true;
					rightButton.interactable = false;
					break;
				}
		}
	}

	const int VIEW_DEFAULT_RATE = 60;
	const int VIEW_MAX_RATE = 80;
    const int VIEW_MIN_RATE = 30;
    const int RATE = 10;

    void Look()
    {
        if (isZoom) { return; }
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
        monitorNum--;
		cam.transform.position = monitor_Trans_Array[monitorNum].transform.position;
		cam.transform.eulerAngles = monitor_Trans_Array[monitorNum].eulerAngles;
        ChangeButtonState();
	}
	public void PushRightButton()
	{
        monitorNum++;
		cam.transform.position = monitor_Trans_Array[monitorNum].transform.position;
		cam.transform.eulerAngles = monitor_Trans_Array[monitorNum].eulerAngles;
        ChangeButtonState();
	}
    public void PushCancelButton()
    {
        isZoom = false;
        canZoom = false;
		cam.gameObject.transform.localPosition = Vector3.zero;
        cam.transform.localEulerAngles = currentRotation;
	}
	void Zoom()
	{
		if (Input.GetKey(KeyCode.Q))
		{
			//�g��
			if (cameraView > VIEW_MIN_RATE)
			{
				cameraView -= Time.deltaTime * RATE;
				cam.fieldOfView = cameraView;
			}
		}
		if (Input.GetKey(KeyCode.E))
		{
			//�g��
			if (cameraView < VIEW_MAX_RATE)
			{
				cameraView += Time.deltaTime * RATE;
				cam.fieldOfView = cameraView;
			}
		}
	}

}
