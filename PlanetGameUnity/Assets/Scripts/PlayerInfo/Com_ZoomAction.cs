using UnityEngine;
//using static UnityEditor.Timeline.TimelinePlaybackControls;
//using UnityEngine.InputSystem.Controls;
using System.Collections;

public class Com_ZoomAction : MonoBehaviour,I_SearchAction
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField,Header("モニター拡大時にUIを操作するクラス")] CommanderUIController commanderUIController;

	[SerializeField, Header("モニター画面時のゲームUI")] GameObject gameUIPanel;
	[SerializeField, Header("Agent側のチャット欄")] GameObject chatUIPanel;
	[SerializeField, Header("エージェント帰還用UI")] GameObject agentReturnUIPanel;
	[SerializeField] GameObject agentView;
	[SerializeField, Header("表示しないガイドUI")] GameObject age_GuideUI;
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

		//Debug.Log("Commander側でSearchの処理が始まった");
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
				//この秒数が0以下になったときはUIは出したけど拡大はしなかったということ
				//canZoomをfalseにしてUIを全て非表示にする

				//コルーチンの起動
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

		//inputからの信号は1,2,3のどれかしか入力はないのでCastしても大丈夫
		int keyNum = int.Parse(keyName);

		//inputから入力された値と画面に出ているUIが一致している時のみZoom
		//キーボードは1～3だけどMonitorNumは配列にも使うから0～2になってる
		if (keyNum == monitorNum + 1)
		{
			Zoom();
		}
		else
		{
			Debug.Log("一致しなかった");
		}
	}

	private void Zoom()
	{
		//Debug.Log("dededed");
		cursorController.Show();
		///Zoomのボタンが押されたらCameraをそれぞれのモニターの前に移動させて
		///押されたモニターの番号に応じてゲームのUIの状態を変える
		commander.ZoomStart(monitorNum);

		commanderUIController.ChangeButtonState(monitorNum);

		gameUIPanel.SetActive(true);

		StopCoroutine(selectCoroutine);

		monitorGuidePanel.SetActive(false);

		//ゲームのUIを非表示にする
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
