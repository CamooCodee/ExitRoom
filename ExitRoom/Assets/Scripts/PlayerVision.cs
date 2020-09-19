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

	#endregion

	#region UnityFunctions

	bool firstFrame = true;

	private void Update()
	{ 
		inVision = GetVisionObject();

		if(firstFrame && inVision == null)
		{
			GameEvents.current.Player.PlayerVisionExit();
		}

		if(inVision != previousVision && inVision != null)
		{
			GameEvents.current.Player.PlayerVisionEnter();
		}
		else if (inVision != previousVision)
		{
			GameEvents.current.Player.PlayerVisionExit();
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
