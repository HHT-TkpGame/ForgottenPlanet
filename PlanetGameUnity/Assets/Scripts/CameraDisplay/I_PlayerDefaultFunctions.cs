using UnityEngine;

public interface I_PlayerDefaultFunctions 
{
	void Init();
	void SetStartPos(Vector3 pos);
	void Move(Vector2 moveAxis);
	void Look(Vector2 lookAxis);
}
