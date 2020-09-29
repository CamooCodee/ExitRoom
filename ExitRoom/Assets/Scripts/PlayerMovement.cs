using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TIME BTW JUMPS
public class PlayerMovement : MonoBehaviour
{
	#region Variables
	public Rigidbody rb;
	public Transform orientation;
	public Animator camAnim;

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
	public float jumpThresholdTime;
	float jumpThresholdStartTime;
	[Space(8f)]
	public float groundCheckRadius = .4f;
	public LayerMask whatIsGround;

	Vector3 deltaPosition;
	bool pressedJump;
	bool isJumping;
	float lastLandingVelocity = 0f;
	float speed;
	float extraLandingMultipl = 1f;
	bool canMove = true;
	bool rayIsGrounded = false;
	bool isGrounded = false;
	GameObject standingOn = null;
	bool canJump = false;
	#endregion

	#region UnityFunctions
	private void Start()
	{
		speed = walkSpeed;
		camAnim.SetFloat("WalkSpeed", .9f);
		jumpThresholdStartTime = jumpThresholdTime;
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

	private void OnCollisionEnter(Collision other)
	{
		if(other.gameObject == standingOn && isJumping)
		{
			isGrounded = true;

			camAnim.SetTrigger("Landing");

			extraLandingMultipl = landingSpeedPercantage;

			if (effectByLandingVelocity)
			{
				extraLandingMultipl *= (1 / Mathf.Abs(lastLandingVelocity)) * effectByLandingVelocityMultipl;
			}

			extraLandingMultipl = Mathf.Clamp01(extraLandingMultipl);
		}
	}
	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject == standingOn)
		{
			isGrounded = false;
		}
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

		bool isWalking = input != Vector2.zero;
		if (camAnim.GetBool("IsWalking") != isWalking)
		{
			camAnim.SetBool("IsWalking", isWalking);
		}

		deltaPosition = ((orientation.forward * input.y) + (orientation.right * input.x)) * extraLandingMultipl * Time.fixedDeltaTime * speed;

		rb.MovePosition(rb.position + deltaPosition);
	}

	void Sprint()
	{
		if (Input.GetKey(KeyCode.LeftShift) && rayIsGrounded)
		{
			camAnim.SetFloat("WalkSpeed", 1.3f);
			speed = runSpeed;
		}
		else if (rayIsGrounded)
		{
			camAnim.SetFloat("WalkSpeed", .9f);
			speed = walkSpeed;
		}
	}

	void JumpInput()
	{
		if (Input.GetKeyDown(KeyCode.Space) && canJump)
		{
			pressedJump = true;
			isJumping = true;
		}
	}
	void Jump()
	{
		if (pressedJump)
		{
			camAnim.SetTrigger("Jumping");
			rb.velocity += Vector3.up * jumpForce;
			pressedJump = false;
		}
	}

	void HandleGround()
	{
		RaycastHit hit;

		bool _isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckRadius, whatIsGround);

		if (hit.collider != null) standingOn = hit.collider.gameObject;
		else standingOn = null;

		if(_isGrounded && rayIsGrounded)
		{
			lastLandingVelocity = rb.velocity.y;
		}

		rayIsGrounded = _isGrounded;
		HandleJumpThreshold();
	}
	void HandleJumpThreshold()
	{
		if (isGrounded && jumpThresholdTime <= 0f) isJumping = false;
		if (isGrounded) jumpThresholdTime = jumpThresholdStartTime;

		if(!rayIsGrounded && jumpThresholdTime >= 0f)
		{
			jumpThresholdTime -= Time.deltaTime;
		}

		canJump = !isJumping || rayIsGrounded;
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
