using UnityEngine;
//using static UnityEditor.Timeline.TimelinePlaybackControls;
//using UnityEngine.InputSystem.Controls;
using System.Collections;

public class Com_ZoomAction : MonoBehaviour,I_SearchAction
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField,Header("���j�^�[�g�厞��UI�𑀍삷��N���X")] CommanderUIController commanderUIController;

	[SerializeField, Header("���j�^�[��ʎ��̃Q�[��UI")] GameObject gameUIPanel;
	[SerializeField, Header("Agent���̃`���b�g��")] GameObject chatUIPanel;
	[SerializeField, Header("�G�[�W�F���g�A�җpUI")] GameObject agentReturnUIPanel;
	[SerializeField] GameObject agentView;
	[SerializeField, Header("�\�����Ȃ��K�C�hUI")] GameObject age_GuideUI;
	[SerializeField]CursorController cursorController;
	[SerializeField] GameObject monitorGuidePanel;

	Commander commander;

	float rayDistance = 5f;

	int monitorNum=0;

	const int MAX_MONITORS = 3;
	const float MAX_CANZOOM_TIME = 5;

	Coroutine selectCoroutine;

	public void SetUp(Commander commander)
    {
        this.commander = commander;
        commanderUIController.SetUp(this);
		gameUIPanel.SetActive(false);
		chatUIPanel.SetActive(false);
		agentReturnUIPanel.SetActive(false);
		agentView.SetActive(false);
		age_GuideUI.SetActive(false);
		monitorGuidePanel.SetActive(false);
	} 

    public void OnSearchStarted()
    {

		if (commander.State == Commander.zoomState.ZoomedIn) { return; }

		//Debug.Log("Commander����Search�̏������n�܂���");
		Ray ray = new Ray(commander.Cam.transform.position, commander.Cam.transform.forward);

		if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
		{
			CommanderMonitor monitor = hit.collider.GetComponent<CommanderMonitor>();
			if (monitor != null)
			{
				commanderUIController.ChangePopUIState(monitorNum,false);

				monitorNum = monitor.Num;

				monitorGuidePanel.SetActive(true);
				monitorGuidePanel.GetComponent<MonitorGuide>().SetGuideUI(monitorNum);

				commanderUIController.ChangePopUIState(monitorNum, true);


				commander.SetZoomState(Commander.zoomState.Select);
				//���̕b����0�ȉ��ɂȂ����Ƃ���UI�͏o�������Ǌg��͂��Ȃ������Ƃ�������
				//canZoom��false�ɂ���UI��S�Ĕ�\���ɂ���

				//�R���[�`���̋N��
				if (selectCoroutine != null)
				{
					StopCoroutine(selectCoroutine);
				}

				selectCoroutine = StartCoroutine(SelectingTimer(MAX_CANZOOM_TIME));
				//canZoom_Time = MAX_CANZOOM_TIME;
			}
		}
	}

	public void OnZoomPerformed(string keyName)
	{
		if (commander.State != Commander.zoomState.Select) { return; }

		//Debug.Log("keyName" + keyName);
		//Debug.Log("monitorNum" + monitorNum);

		//input����̐M����1,2,3�̂ǂꂩ�������͂͂Ȃ��̂�Cast���Ă����v
		int keyNum = int.Parse(keyName);

		//input������͂��ꂽ�l�Ɖ�ʂɏo�Ă���UI����v���Ă��鎞�̂�Zoom
		//�L�[�{�[�h��1�`3������MonitorNum�͔z��ɂ��g������0�`2�ɂȂ��Ă�
		if (keyNum == monitorNum + 1)
		{
			Zoom();
		}
		else
		{
			Debug.Log("��v���Ȃ�����");
		}
	}

	private void Zoom()
	{
		//Debug.Log("dededed");
		cursorController.Show();
		///Zoom�̃{�^���������ꂽ��Camera�����ꂼ��̃��j�^�[�̑O�Ɉړ�������
		///�����ꂽ���j�^�[�̔ԍ��ɉ����ăQ�[����UI�̏�Ԃ�ς���
		commander.ZoomStart(monitorNum);

		commanderUIController.ChangeButtonState(monitorNum);

		gameUIPanel.SetActive(true);

		StopCoroutine(selectCoroutine);

		monitorGuidePanel.SetActive(false);

		//�Q�[����UI���\���ɂ���
		for (int i = 0; i < MAX_MONITORS; i++)
		{
			commanderUIController.ChangePopUIState(i, false);
		}
	}

	IEnumerator SelectingTimer(float time)
	{
		if (commander.State != Commander.zoomState.Select) { yield break; }
		yield return new WaitForSeconds(time);

		commander.SetZoomState(Commander.zoomState.Default);

		monitorGuidePanel.SetActive(false);

		for (int i = 0; i < MAX_MONITORS; i++)
		{
			commanderUIController.ChangePopUIState(i, false);
		}
	}

	public void PushLeftButton()
	{
		commanderUIController.SetGraphicArray(monitorNum,false);
		monitorNum--;
		commander.SetCameraTrans(monitorNum);
		commanderUIController.ChangeButtonState(monitorNum);
	}
	public void PushRightButton()
	{
		commanderUIController.SetGraphicArray(monitorNum, false);
		monitorNum++;
		commander.SetCameraTrans(monitorNum);
		commanderUIController.ChangeButtonState(monitorNum);
	}
	public void PushCancelButton()
	{
		commander.SetZoomState(Commander.zoomState.Default);
		commander.ZoomCancel();
		gameUIPanel.SetActive(false);
	}
	public void PushAgentReturnButton()
	{
		agentReturnUIPanel.SetActive(true);
		agentReturnUIPanel.GetComponent<AgentReturnUI>().UITextSetting();

	}



	public void OnSearchCanceled()
    {

	}
}
