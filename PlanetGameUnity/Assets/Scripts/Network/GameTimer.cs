using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    const float TIMER = 300f;
    float timer = TIMER;
    public event Action OnTimerEnded;
    bool isTimerEnded;
    void Start()
    {
        
    }

    void Update()
    {
        if (isTimerEnded) { return; }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            isTimerEnded = true;
            OnTimerEnded?.Invoke();
        }
    }
}
