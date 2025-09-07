using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatterySlideBehavior : MonoBehaviour
{
	Slider timerGauge;   //c‚èŠÔƒQ[ƒW
	[SerializeField] GameTimer timer;

	void Start()
	{
		timerGauge = GetComponent<Slider>();
		timerGauge.maxValue = timer.MaxTimer;
	}

	void Update()
	{
		if (timer.Timer < 0) { return; }
		timerGauge.value = timer.Timer;
	}
}
