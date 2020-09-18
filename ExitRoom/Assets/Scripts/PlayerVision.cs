using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{
	#region Variables
	public GameObject cam;
	public GameObject inVision;
	GameObject previousVision;

	[Space(15f)]
	public float visionRange;

	[Space(15f)]
	public LayerMask whatIsInteractable;


	public delegate void EmptyVisionDelegate();
	public event EmptyVisionDelegate OnVisionEnter;
	public event EmptyVisionDelegate OnVisionExit;
	#endregion

	#region UnityFunctions

	bool firstFrame = true;

	private void Update()
	{ 
		inVision = GetVisionObject();

		if(firstFrame && inVision == null)
		{
			OnVisionExit?.Invoke();
		}

		if(inVision != previousVision && inVision != null)
		{
			OnVisionExit?.Invoke();
			OnVisionEnter?.Invoke();
		}
		else if (inVision != previousVision)
		{
			OnVisionExit?.Invoke();
		}

		previousVision = inVision;

		firstFrame = false;
	}
	#endregion

	#region UniqueFunctions

	public GameObject GetVisionObject()
	{
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, visionRange, whatIsInteractable))
		{
			return hit.collider.gameObject;
		}
		else
		{
			return null;
		}
	}
	#endregion
	
	#region DebugFunctions
	
	#endregion
	
	#region MathOrLibaryRework
	
	#endregion
}
