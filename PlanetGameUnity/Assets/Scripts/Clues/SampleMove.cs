using UnityEngine;

public class SampleMove : MonoBehaviour
{
	int speed = 10;
	int direction = 1;
	const int MAXPOS_X = 20;
	bool onLeft;

	Vector3 currentPos;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		currentPos = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(new Vector3(speed * Time.deltaTime * direction, 0, 0));
		if (transform.position.x > MAXPOS_X-currentPos.x && !onLeft)
		{
			direction *= -1;
			onLeft = true;
		}
		else if (transform.position.x < MAXPOS_X+currentPos.x * -1 && onLeft)
		{
			direction *= -1; onLeft = false;
		}
	}
}
