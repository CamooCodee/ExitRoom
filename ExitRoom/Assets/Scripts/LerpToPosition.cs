using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToPosition : MonoBehaviour
{
	#region Variables
	public Transform target;
	[Range(0.0001f, .95f)]
	public float smoothness;
	public bool equalOnStart = true;
	#endregion

	#region UnityFunctions
	private void Start()
	{
		if (equalOnStart) transform.position = target.position;
	}

	private void LateUpdate()
	{
		LerpToTarget();
	}
	#endregion

	#region UniqueFunctions
	void LerpToTarget()
	{
		Vector3 deltaPos = target.position;

		if (Vector3.Distance(target.position, transform.position) > .02f)
		{
			deltaPos = Vector3.Lerp(target.position, transform.position, smoothness);
		}

		transform.position = deltaPos;
	}
	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}
