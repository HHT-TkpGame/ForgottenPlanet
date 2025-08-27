using UnityEngine;
using UnityEngine.UI;

public class Com_ClueInfo : MonoBehaviour
{
	//�����������ǂ���
	//bool isSolved=false;

	Sprite clueImage;
	Sprite codeImage;

	Image infoImage;

	InputFieldManager inputManager;

	string codeAns;
	public string CodeAns => codeAns;

	public void SetPanelImages(Sprite clueImage, Sprite codeImage, string codeAns)
	{
		this.clueImage = clueImage;
		//this.codeImage = codeImage;
		this.codeAns = codeAns;
		infoImage = GetComponent<Image>();
		infoImage.sprite = codeImage;
	}

	//panelObj��Image���ǂ����ɂ��邩�����[�J���ϐ����g���Ĕ��肷��
	public bool VerifyAnswer(string answer)
	{
		Debug.Log("������" + codeAns + "���͂�" + answer);
		if (codeAns == answer)
		{
			Debug.Log("eeeee");
			infoImage.sprite = clueImage;
			return true;
		}
		return false;
	}
}
