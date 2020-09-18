using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	#region Variables
	public CharacterController controller;
	public Transform orientation;
	public Transform groundCheck;

	[Space(20f)]

	public float speed = 12f;
	public float jumpHeight = 4f;
	public float gravity = -9.81f;
	[Space(8f)]
	public float groundCheckRadius = .4f;
	public LayerMask whatIsGround;

	Vector3 velocity;
	bool canMove = true;
	bool isGrounded = false;
	bool isSprinting = false;
	#endregion

	#region UnityFunctions
	private void Update()
	{
		if(canMove)Move();
	}
	#endregion

	#region UniqueFunctions

	Vector2 GetInput()
	{
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		return new Vector2(x, y);
	}

	private void Move()
	{
		Sprint();
		Vector2 input = GetInput();

		Vector3 move = orientation.right * input.x + orientation.forward * input.y;

		controller.Move(move * speed * Time.deltaTime);

		CheckGround();
		HandleVertical(CheckGround());
	}

	void Sprint()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			isSprinting = true;
			speed *= 1.8f;
		}
		else if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			isSprinting = false;
			speed /= 1.8f;
		}
	}

	void Jump()
	{
		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}
	}

	void HandleVertical(bool isOnGround)
	{
		if (isOnGround && velocity.y < 0f)
		{
			velocity.y = -2f;
		}

		Jump();

		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}

	bool CheckGround()
	{
		isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, whatIsGround);
		return isGrounded;
	}

	public void SetActive(bool state)
	{
		canMove = state;
	}
	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}
