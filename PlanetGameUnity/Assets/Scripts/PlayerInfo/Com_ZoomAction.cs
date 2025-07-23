using UnityEngine;

public class Com_ZoomAction : MonoBehaviour,I_SearchAction
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void OnSearchStarted()
    {
        Debug.Log("Commander‘¤‚ÅSearch‚Ìˆ—‚ªn‚Ü‚Á‚½");
    }
	public void OnSearchCanceled()
    {
		Debug.Log("Commander‘¤‚ÅSearch‚Ìˆ—‚ªI‚í‚Á‚½");
	}
}
