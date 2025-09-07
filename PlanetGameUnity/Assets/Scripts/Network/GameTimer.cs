using System;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float Timer => timer;
    [SerializeField, Header("êßå¿éûä‘(sec)")] float timer;
    public float MaxTimer {  get; private set; }
    public event Action OnTimerEnded;
    bool isTimerEnded;
    [SerializeField] TMP_Text txtTimer;
    void Awake()
    {
        txtTimer.text = "";
        MaxTimer = timer;
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
