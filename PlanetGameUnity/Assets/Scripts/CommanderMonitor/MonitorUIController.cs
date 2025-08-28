using UnityEngine;
using UnityEngine.UI;

public class MonitorUIController : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created

	const int LEFT = 0;
	const int CENTER = 1;
	const int RIGHT = 2;

	const int MAX_MONITORS = 3;
	//monitorè„ÇÃImageÇÃîzóÒÇ±ÇÍ5Ç¬ÇÕUIConçsÇ´?
	[SerializeField] GameObject[] monitor_PopUI_Array = new GameObject[MAX_MONITORS];
	[SerializeField] GraphicRaycaster[] monitor_Graphic_Array = new GraphicRaycaster[MAX_MONITORS];
	[SerializeField] GameObject defaultCanvas;
	[SerializeField] Button leftButton;
	[SerializeField] Button rightButton;

	Com_ZoomAction zoomAct;
    //UIånÇÃèâä˙ê›íËÇ‡
    public void SetUp(Com_ZoomAction zoomAct)
    {
        this.zoomAct = zoomAct;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
