using System.Collections;
using UnityEngine;

public class CursorController : MonoBehaviour
{
	public void Lock()
	{
		Cursor.lockState =  CursorLockMode.Locked;
	}
	public void Show()
	{
		Cursor.lockState = CursorLockMode.None;
	}
}
