using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	#region Variables
	PlayerVision playerVision;
	public event Action onInteract;
	#endregion

	#region UnityFunctions
	public void Awake()
	{
		playerVision = GameEvents.current.playerVision;
		GameEvents.current.Player.OnInteractionAttempt += TryInteract;
	}
	#endregion

	#region UniqueFunctions
	void TryInteract()
	{
		if(playerVision.inVision == gameObject)
		{
			Interact();
		}
	}

	void Interact()
	{
		onInteract?.Invoke();
	}

	private void OnDestroy()
	{
		GameEvents.current.Player.OnInteractionAttempt -= TryInteract;
	}
	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}
