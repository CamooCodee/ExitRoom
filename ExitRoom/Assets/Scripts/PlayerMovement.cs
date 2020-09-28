using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	#region Variables
	public Rigidbody rb;
	public Transform orientation;

	[Space(20f)]

	public float walkSpeed = 12f;
	public float runSpeed = 12f;
	[Space(5f)]
	[Range(0f, 1f)]
	public float landingSpeedPercantage;
	public float onLandingSpeedIncrease;
	[Range(0f, 10f)]
	public float effectByLandingVelocityMultipl;
	public bool effectByLandingVelocity;
	[Space(5f)]
	public float jumpForce = 30f;
	[Space(8f)]
	public float groundCheckRadius = .4f;
	public LayerMask whatIsGround;

	Vector3 deltaPosition;
	bool pressedJump;
	float speed;
	float extraLandingMultipl = 1f;
	bool canMove = true;
	bool isGrounded = false;
	#endregion

	#region UnityFunctions
	private void Start()
	{
		speed = walkSpeed;
	}

	private void FixedUpdate()
	{
		if (!canMove) return;
		Move();
		Jump();
	}
	private void Update()
	{
		if(extraLandingMultipl < 1f)
		{
			extraLandingMultipl += Time.deltaTime * onLandingSpeedIncrease;
		}
		else if (extraLandingMultipl > 1f)
		{
			extraLandingMultipl = Mathf.Clamp01(extraLandingMultipl);
		}

		if (!canMove) return;
		HandleGround();
		Sprint();
		JumpInput();
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
		Vector2 input = GetInput();

		deltaPosition = ((orientation.forward * input.y) + (orientation.right * input.x)) * extraLandingMultipl * Time.fixedDeltaTime * speed;

		rb.MovePosition(rb.position + deltaPosition);
	}

	void Sprint()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			speed = runSpeed;
		}
		else if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			speed = walkSpeed;
		}
	}

	void JumpInput()
	{
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			pressedJump = true;
		}
	}
	void Jump()
	{
		if (pressedJump)
		{
			rb.velocity += Vector3.up * jumpForce;
			pressedJump = false;
		}
	}

	void HandleGround()
	{
		bool _isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckRadius, whatIsGround);

		if (_isGrounded && !isGrounded)
		{
			extraLandingMultipl = landingSpeedPercantage;

			if (effectByLandingVelocity)
			{
				extraLandingMultipl *= (1 / Mathf.Abs(rb.velocity.y)) * effectByLandingVelocityMultipl;
			}

			extraLandingMultipl = Mathf.Clamp01(extraLandingMultipl);
		}

		isGrounded = _isGrounded;
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
