using UnityEngine;

public interface I_SearchAction 
{
    void OnSearchStarted();
    void OnSearchCanceled();
    void OnZoomPerformed(string keyName);
}
