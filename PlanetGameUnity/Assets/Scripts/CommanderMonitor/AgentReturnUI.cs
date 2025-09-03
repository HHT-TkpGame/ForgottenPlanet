using System.Text;
using UnityEngine;

public class AgentReturnUI : MonoBehaviour
{
    bool isLast;

    [SerializeField] GameObject returnText;
	[SerializeField] GameObject returnLastText;

	public void UITextSetting()
    {
        if (!isLast)
        {
            returnLastText.SetActive(false);
			returnText.SetActive(true);
		}
        else
        {
            returnText.SetActive(false);
            returnLastText.SetActive(true);
        }
    }
    public void PushYesButton()
    {
        //isLastの状態に応じて押されたときの効果を変える
        if (!isLast)
        {
            isLast = true;
            UITextSetting();
        }
        else
        {
            ///ゲームを終了
            Debug.Log("ゲーム終了");
        }
    }
    public void PushNoButton()
    {
        isLast=false;
        gameObject.SetActive(false);
    }
}
