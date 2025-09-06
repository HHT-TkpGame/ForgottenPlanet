using UnityEngine;

public class CluesDataGetter : MonoBehaviour
{
    public static CluesDataGetter Instance;
    public ServerCurrentMatchClues Data {  get; private set; }
    public ClueClient ClueClient { get; private set; }
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    public void Dispose()
    {
        Instance = null;
        Destroy(gameObject);
    }
    void Start()
    {
        ClueClient = new ClueClient();
        StartCoroutine(ClueClient.GetClueAndTruth(onSuccess:(res) =>
        {
            Data = res;
        },
        onError:(err) =>
        {
            Debug.Log("‰Šú‰»—pŽè‚ª‚©‚èŽæ“¾Ž¸”s");
        }));
    }
}
