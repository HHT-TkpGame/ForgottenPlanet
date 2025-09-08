using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class MonitorGuide : MonoBehaviour
{
	const int MAX_MONITORNUM = 3;
	[SerializeField] Sprite[] monitorGuides = new Sprite[MAX_MONITORNUM];
	[SerializeField] Image guideImage;
	public void SetGuideUI(int num)
	{
		guideImage.sprite = monitorGuides[num];
	}
}
