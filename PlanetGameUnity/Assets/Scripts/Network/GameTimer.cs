using System;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    const float TIMER = 5f;
    [SerializeField, Header("êßå¿éûä‘(sec)")] float timer;
    public event Action OnTimerEnded;
    bool isTimerEnded;
    [SerializeField] TMP_Text txtTimer;
    void Awake()
    {
        txtTimer.text = "";
    }

    void Update()
    {
        if (isTimerEnded) { return; }
        timer -= Time.deltaTime;
        txtTimer.text = timer.ToString("f0");
        if (timer < 0)
        {
            isTimerEnded = true;
            OnTimerEnded?.Invoke();
        }
    }
}
