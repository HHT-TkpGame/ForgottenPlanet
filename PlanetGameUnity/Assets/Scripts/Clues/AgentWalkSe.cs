using UnityEngine;

public class AgentWalkSe : MonoBehaviour
{
    AudioSource se;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        se = GetComponent<AudioSource>();
    }

    public void SeStart()
    {
        if (!se.isPlaying)
        {
            se.Play();
        }
    }
}
