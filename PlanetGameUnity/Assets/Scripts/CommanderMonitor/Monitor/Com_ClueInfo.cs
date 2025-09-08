using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Com_ClueInfo : MonoBehaviour
{
	//正解したかどうか
	//bool isSolved=false;
	[SerializeField,Header("手がかりのテキスト")]TMP_Text clueText;

	[SerializeField] ChatUIController chatUIController;
	Sprite clueImage;
	Sprite codeImage;

	Image infoImage;

	InputFieldManager inputManager;

	string codeAns;
	public string CodeAns => codeAns;

	bool isCleared;
	public bool IsCleared => isCleared;

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
			chatUIController.DisplayChat("暗号の解読に成功した！");
			infoImage.sprite = clueImage;
			clueText.enabled = true;
			isCleared = true;
			return true;
		}
		return false;
	}
}
