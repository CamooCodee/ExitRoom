using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildRotationPretender : MonoBehaviour
{
	#region Variables
	public Transform target;
	public bool x, y, z;
	#endregion

	#region UnityFunctions
	private void LateUpdate()
	{
		if (x) transform.localRotation = Quaternion.Euler(target.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);

		if (y) transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, target.localEulerAngles.y, transform.localEulerAngles.z);

		if (z) transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, target.localEulerAngles.z);
	}
	#endregion

	#region UniqueFunctions

	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}
