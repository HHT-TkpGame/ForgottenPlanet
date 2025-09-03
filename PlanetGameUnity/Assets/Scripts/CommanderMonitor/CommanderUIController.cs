using UnityEngine;
using UnityEngine.UI;

public class CommanderUIController : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created

	const int LEFT = 0;
	const int CENTER = 1;
	const int RIGHT = 2;

	const int MAX_MONITORS = 3;

	[SerializeField] GameObject[] monitor_PopUI_Array = new GameObject[MAX_MONITORS];
	[SerializeField] GraphicRaycaster[] monitor_Graphic_Array = new GraphicRaycaster[MAX_MONITORS];
	//[SerializeField] GameObject defaultCanvas;
	[SerializeField] Button leftButton;
	[SerializeField] Button rightButton;

	Com_ZoomAction zoomAct;
    //UIånÇÃèâä˙ê›íËÇ‡
    public void SetUp(Com_ZoomAction zoomAct)
    {
        this.zoomAct = zoomAct;
		for (int i = 0;i< monitor_PopUI_Array.Length;i++)
		{
			ChangePopUIState(i, false);
		}
    }

	public void ChangePopUIState(int num,bool active)
	{
		monitor_PopUI_Array[num].SetActive(active);
	}
	public void ChangeButtonState(int monitorNum)
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
	public void SetGraphicArray(int num, bool isActive)
	{
		Debug.Log(isActive);
		monitor_Graphic_Array[num].enabled = isActive;
	}
}
