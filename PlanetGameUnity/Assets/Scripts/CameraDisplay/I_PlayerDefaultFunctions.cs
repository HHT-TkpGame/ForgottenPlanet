using UnityEngine;

public interface I_PlayerDefaultFunctions 
{
	void Init();
	void SetStartPos();
	void Move(Vector2 moveAxis);
	void Look(Vector2 lookAxis);
}
