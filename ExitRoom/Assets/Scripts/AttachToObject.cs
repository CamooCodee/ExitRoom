using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToObject : MonoBehaviour
{
	#region Variables
	public Transform toAttachTo;
	[Space(15f)]
	public Vector3 positionOffset;
	#endregion

	#region UnityFunctions
	private void LateUpdate()
	{
		transform.position = toAttachTo.position + positionOffset;
		transform.rotation = toAttachTo.transform.rotation;
	}
	#endregion

	#region UniqueFunctions

	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}
