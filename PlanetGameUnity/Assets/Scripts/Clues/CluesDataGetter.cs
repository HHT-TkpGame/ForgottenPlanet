using UnityEngine;

public class CluesDataGetter : MonoBehaviour
{
    public static CluesDataGetter Instance;
    public ServerCurrentMatchClues Data {  get; private set; }
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
    void Start()
    {
        ClueClient clueClient = new ClueClient();
        StartCoroutine(clueClient.GetClueAndTruth(onSuccess:(res) =>
        {
            Data = res;
        },
        onError:(err) =>
        {
            Debug.Log("�������p�肪����擾���s");
        }));
    }
}
