using UnityEngine;

public class SearchActionController// : MonoBehaviour
{
    I_SearchAction iSearch;
    public void Init(I_SearchAction iSearch)
    {
        this.iSearch = iSearch;
    }

    public void OnSearchStarted()
    {
        //‹¤’Ê‚Ìˆ—‚ª‚ ‚éê‡‚±‚±‚É’Ç‰Á

        iSearch.OnSearchStarted();
    }
	public void OnSearchCanceled()
	{
		//‹¤’Ê‚Ìˆ—‚ª‚ ‚éê‡‚±‚±‚É’Ç‰Á

		iSearch.OnSearchCanceled();
	}
}
