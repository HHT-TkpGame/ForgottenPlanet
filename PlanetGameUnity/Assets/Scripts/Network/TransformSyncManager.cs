using UnityEngine;

public class TransformSyncManager : MonoBehaviour
{
    ITransformStrategy.ITransformSenderStrategy iSender;
    ITransformStrategy.ITransformGetterStrategy iGetter;

    [SerializeField] TransformSender sender;
    [SerializeField] TransformGetter getter;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (MatchingManager.IsCommander)
        {
            iGetter = getter;
            iGetter.Initialize();
        }
        else
        {
            iSender = sender;
            iSender.Initialize();
        }
    }
     
    // Update is called once per frame
    void Update()
    {
        
    }
}
