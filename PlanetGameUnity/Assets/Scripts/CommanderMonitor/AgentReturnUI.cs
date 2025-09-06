using System.Text;
using UnityEngine;

public class AgentReturnUI : MonoBehaviour
{
    bool isLast;
    InGameEnder ender;
    [SerializeField] GameObject returnText;
	[SerializeField] GameObject returnLastText;
    public void Init(InGameEnder ender)
    {
        this.ender = ender;
    }
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
        //isLast‚Ìó‘Ô‚É‰‚¶‚Ä‰Ÿ‚³‚ê‚½‚Æ‚«‚ÌŒø‰Ê‚ğ•Ï‚¦‚é
        if (!isLast)
        {
            isLast = true;
            UITextSetting();
        }
        else
        {
            //ƒQ[ƒ€‚ğI—¹
            ender.IncrementState();
        }
    }
    public void PushNoButton()
    {
        isLast=false;
        gameObject.SetActive(false);
    }
}
