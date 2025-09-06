using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Com_ClueInfo : MonoBehaviour
{
	//�����������ǂ���
	//bool isSolved=false;
	[SerializeField,Header("�肪����̃e�L�X�g")]TMP_Text clueText;

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
		clueText.enabled = false;
	}

	//panelObj��Image���ǂ����ɂ��邩�����[�J���ϐ����g���Ĕ��肷��
	public bool VerifyAnswer(string answer)
	{
		//Debug.Log("������" + codeAns + "���͂�" + answer);
		if (codeAns == answer)
		{
			infoImage.sprite = clueImage;
			clueText.enabled = true;
			return true;
		}
		return false;
	}
}
