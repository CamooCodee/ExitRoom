using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
	#region Inspector Variables
	[Header("References: ")]
	public Transform cam;
	public Transform orientation;
	[Header("Settings: ")]
	public float sensitivity = 20f;
	#endregion

	#region Private Variables
	float xRotation;
	float yRotation;
	float mouseX;
	float mouseY;

	bool canLook = true;
	#endregion

	private void Start()
	{
		xRotation = cam.eulerAngles.x;
		yRotation = cam.eulerAngles.y;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		if (!canLook) return;
		MouseInput();
		Look();
	}

	void MouseInput()
	{
		mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
		mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
	}

	void Look()
	{
		
		//Find current look rotation
		yRotation += mouseX;

		//Rotate, and also make sure we dont over- or under-rotate.
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		//Perform the rotations
		cam.transform.localEulerAngles = new Vector2(xRotation, 0f);
		orientation.transform.localEulerAngles = new Vector2(0f, yRotation);
	}

	public void SetActive(bool state)
	{
		canLook = state;
	}
}
