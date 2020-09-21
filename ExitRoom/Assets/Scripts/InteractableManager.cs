using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
	public static InteractableManager current;

	#region Variables
	public PlayerVision playerVision;
	public event Action onInteractionAttempt;
	#endregion

	#region UnityFunctions
	private void Awake()
	{
		if (current != null)
		{
			Destroy(gameObject);
			//Debug.LogError("Make sure theres only one InteractableManager in the current scene");
		}
		else
		{
			current = this;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			onInteractionAttempt?.Invoke();
		}
	}
	#endregion

	#region UniqueFunctions

	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}
