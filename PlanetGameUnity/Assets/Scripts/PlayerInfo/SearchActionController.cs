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
        //���ʂ̏���������ꍇ�����ɒǉ�

        iSearch.OnSearchStarted();
    }
	public void OnSearchCanceled()
	{
		//���ʂ̏���������ꍇ�����ɒǉ�

		iSearch.OnSearchCanceled();
	}
}
