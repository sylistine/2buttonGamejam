using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	//The up-down and left-right buttons toggle back and forth after pressed
	//So these fields store the current direction of either button
	bool isButtonDirUp = true;
	bool isButtonDirLeft = true;
	
	//Because of the unique input style, Input.GetButtonDown and Input.GetButtonUp were better for flow control than Input.GetButton
	//So these fields track button presses
	bool udButtonIsPressed = false;
	bool lrButtonIsPressed = false;

	//This needs to persist between updates
	//Initialized to (0, 0, 0) for safety
	Vector3 moveDirection = Vector3.zero;

	/*Public variables*/
	//These are opened for the editor to allow quicker tweaking during play test

	//moveSpeed should be obvious
	public float moveSpeed = 2f;

	//inputCooldownCost is the amount of time (in seconds) you must wait between inputs.
	//This is added to inputCooldown after Input.GetButtonUp
	public float inputCooldownCost = 0.5f;
	
	//inputCooldown is the current time (in seconds) you currently have to wait before you can move the crane again
	//This counts down every frame. At 0 you can input again.
	public float inputCooldown = 0f;

	void Update () {
		//Subtract Time.deltaTime if the input is greater than zero
		if(inputCooldown > 0)
		{
			//If remaining cooldown time is > Time.deltaTime, subtract Time.deltaTime, else set to zero (or inputCooldown will become negative and throw errors)
			inputCooldown = (inputCooldown > Time.deltaTime) ? inputCooldown - Time.deltaTime : 0;
		}

		//If neither button is being pressed and cooldown is 0, let the player press a button
		if(!udButtonIsPressed && !lrButtonIsPressed && inputCooldown <= 0)
		{
			//no race-state. the UpDown input will always come first if both buttons are pressed on the same frame.
			//todo: may create trouble later when needs to read both buttons at the same time (to drop the crane)
			if(Input.GetButton ("UpDown"))
			{
				//record that a button is being pressed
				udButtonIsPressed = true;
				//get the new move direction based on the current toggle setting
				moveDirection = (isButtonDirUp) ? Vector3.forward : Vector3.back;
				//toggle the setting for next time
				isButtonDirUp = !isButtonDirUp;
			}
			else if(Input.GetButton ("LeftRight"))
			{
				lrButtonIsPressed = true;
				moveDirection = (isButtonDirLeft) ? Vector3.left : Vector3.right;
				isButtonDirLeft = !isButtonDirLeft;
			}
		}

		//if either button is pressed, move the crane
		if(udButtonIsPressed || lrButtonIsPressed)
		{
			//move the crane manually - bound to world space (rotating everything 90deg on y-axis would effectively swap UpDown with LeftRight)
			this.transform.position += moveDirection * moveSpeed * Time.deltaTime;

			//check for a button up. Only checks the udButton or lrButton if it's marked as on. 
			if(udButtonIsPressed && Input.GetButtonUp ("UpDown"))
			{
				//record that a button is not being pressed
				udButtonIsPressed = false;
				//set cooldown
				inputCooldown = inputCooldownCost;
			}
			else if(lrButtonIsPressed && Input.GetButtonUp ("LeftRight"))
			{
				lrButtonIsPressed = false;
				inputCooldown = inputCooldownCost;
			}
		}
	}
}
