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
	public float rotationSmoothSpeed = 10f;
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
		mouseX = Input.GetAxis("Mouse X") * sensitivity;
		mouseY = Input.GetAxis("Mouse Y") * sensitivity;
	}

	void Look()
	{
		yRotation += mouseX * sensitivity;
		xRotation += mouseY * sensitivity;

		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		Quaternion camTargetRotation = Quaternion.Euler(-xRotation, cam.eulerAngles.y, cam.eulerAngles.z);
		Quaternion orientationTargetRotation = Quaternion.Euler(0, yRotation, 0);

		orientation.rotation = Quaternion.Lerp(orientation.rotation, orientationTargetRotation, Time.deltaTime * rotationSmoothSpeed);
		cam.rotation = Quaternion.Lerp(cam.rotation, camTargetRotation, Time.deltaTime * rotationSmoothSpeed);
	}

	public void SetActive(bool state)
	{
		canLook = state;
	}
}
