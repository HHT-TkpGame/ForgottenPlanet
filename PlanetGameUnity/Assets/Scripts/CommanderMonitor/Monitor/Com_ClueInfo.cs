using UnityEngine;
using UnityEngine.UI;

public class Com_ClueInfo : MonoBehaviour
{
	//³‰ğ‚µ‚½‚©‚Ç‚¤‚©
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

	//panelObj‚ÌImage‚ğ‚Ç‚Á‚¿‚É‚·‚é‚©‚ğƒ[ƒJƒ‹•Ï”‚ğg‚Á‚Ä”»’è‚·‚é
	public bool VerifyAnswer(string answer)
	{
		Debug.Log("³‰ğ‚Í" + codeAns + "“ü—Í‚Í" + answer);
		if (codeAns == answer)
		{
			Debug.Log("eeeee");
			infoImage.sprite = clueImage;
			return true;
		}
		return false;
	}
}
