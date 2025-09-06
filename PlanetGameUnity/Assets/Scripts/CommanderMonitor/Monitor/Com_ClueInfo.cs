using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Com_ClueInfo : MonoBehaviour
{
	//正解したかどうか
	//bool isSolved=false;
	[SerializeField,Header("手がかりのテキスト")]TMP_Text clueText;

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

	//panelObjのImageをどっちにするかをローカル変数を使って判定する
	public bool VerifyAnswer(string answer)
	{
		//Debug.Log("正解は" + codeAns + "入力は" + answer);
		if (codeAns == answer)
		{
			infoImage.sprite = clueImage;
			clueText.enabled = true;
			return true;
		}
		return false;
	}
}
