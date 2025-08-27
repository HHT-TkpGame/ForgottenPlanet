using UnityEngine;
using UnityEngine.UI;

public class FileBehavior : MonoBehaviour
{
	[SerializeField, Header("���g�̃t�@�C���̔ԍ�")] int num;
	MonitorController monitorController;
	Button button;

	public void GetMonitorController(MonitorController controller)
	{
		monitorController = controller;
		button = GetComponent<Button>();
		button.interactable = false;
	}
	public void SendMyNumber()
	{
		monitorController.DisplayInfoPanel(num);
	}
}
