using UnityEngine;

public class UIPressSounds : MonoBehaviour
{
	AudioSource se;

	private void Start()
	{
		se = GetComponent<AudioSource>();
	}
	public void PressUI()
	{
		se.Play();
	}

}
