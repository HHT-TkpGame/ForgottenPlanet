using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

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
    public void OnZoomPerformed(InputControl control)
    {
        if(control is KeyControl keyControl)
        {
            string keyName = keyControl.name;
			iSearch.OnZoomPerformed(keyName);
		}

    }
}
