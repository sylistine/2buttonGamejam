using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	bool up = true;
	bool left = true;
	bool udButtonIsDown = false;
	bool lrButtonIsDown = false;
	Vector3 moveDirection = Vector3.zero;

	public float moveSpeed = 2f;
	public float inputCooldownCost = 0.5f;
	public float inputCooldown = 0f;

	void Update () {
		if(inputCooldown > 0)
		{
			inputCooldown = (Time.deltaTime < inputCooldown) ? inputCooldown - Time.deltaTime : 0;
		}

		if(!udButtonIsDown && !lrButtonIsDown && inputCooldown <= 0)
		{
			if(Input.GetButton ("UpDown"))
			{
				udButtonIsDown = true;
				moveDirection = (up) ? Vector3.forward : Vector3.back;
				up = !up;
			}
			else if(Input.GetButton ("LeftRight"))
			{
				lrButtonIsDown = true;
				moveDirection = (left) ? Vector3.left : Vector3.right;
				left = !left;
			}
		}

		if(udButtonIsDown || lrButtonIsDown)
		{
			this.transform.position += moveDirection * moveSpeed * Time.deltaTime;

			if(udButtonIsDown && Input.GetButtonUp ("UpDown"))
			{
				udButtonIsDown = false;
				inputCooldown = inputCooldownCost;
			}
			else if(lrButtonIsDown && Input.GetButtonUp ("LeftRight"))
			{
				lrButtonIsDown = false;
				inputCooldown = inputCooldownCost;
			}
		}
	}
}
