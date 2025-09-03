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
        //isLast�̏�Ԃɉ����ĉ����ꂽ�Ƃ��̌��ʂ�ς���
        if (!isLast)
        {
            isLast = true;
            UITextSetting();
        }
        else
        {
            ///�Q�[�����I��
            Debug.Log("�Q�[���I��");
        }
    }
    public void PushNoButton()
    {
        isLast=false;
        gameObject.SetActive(false);
    }
}
