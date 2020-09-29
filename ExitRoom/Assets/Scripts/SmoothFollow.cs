using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
	#region Variables
	public Transform target;
	[Range(0.001f, .95f)]
	public float smoothness;
	public bool equalOnStart = true;
	#endregion

	#region UnityFunctions
	private void Start()
	{
		if (equalOnStart) transform.position = target.position;
	}

	private void Update()
	{
		LerpToTarget();
	}
	#endregion

	#region UniqueFunctions
	Vector3 speed = Vector3.zero;
	void LerpToTarget()
	{
		Vector3 deltaPos = Vector3.SmoothDamp(transform.position, target.position, ref speed, smoothness * 0.1f);

		transform.position = deltaPos;
	}
	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}
