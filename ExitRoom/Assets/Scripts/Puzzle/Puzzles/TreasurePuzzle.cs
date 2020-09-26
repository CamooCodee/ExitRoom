using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class TreasurePuzzle : Puzzle
{
	#region solutionPin
	const string CORRECT_PIN = "1103";
	#endregion

	#region Variables
	string enteredPin = "";
	#endregion

	#region UnityFunctions
	private void Start()
	{
		GetComponent<Interactable>().onInteract += TogglePinOverlay;
		UIManager.current.GetManager<TreasurePinUI>().onPinEnter += CheckPin;
	}
	#endregion

	#region UniqueFunctions
	void TogglePinOverlay()
	{
		if (!isActive)
		{
			UIManager.current.GetManager<PlayerWarningsUI>().ThrowInactivePuzzleWarning();
			return;
		}
		UIManager.current.GetManager<TreasurePinUI>().TogglePinInput();
	}

	void CheckPin(string pin)
	{
		if (!isActive) return;

		enteredPin = pin;
		CheckForCompletion();
	}

	public override void CheckForCompletion()
	{
		if(enteredPin == CORRECT_PIN)
		{
			InvokeOnComplete();
		}
	}
	protected override void DisablePuzzle()
	{
		gameObject.layer = 0;
		Destroy(this);
		Destroy(GetComponent<Interactable>());
	}
	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}
