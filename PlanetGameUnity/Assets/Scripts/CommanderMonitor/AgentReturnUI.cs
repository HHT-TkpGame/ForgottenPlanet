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
        //isLast�̏�Ԃɉ����ĉ����ꂽ�Ƃ��̌��ʂ�ς���
        if (!isLast)
        {
            isLast = true;
            UITextSetting();
        }
        else
        {
            //�Q�[�����I��
            ender.IncrementState();
        }
    }
    public void PushNoButton()
    {
        isLast=false;
        gameObject.SetActive(false);
    }
}
