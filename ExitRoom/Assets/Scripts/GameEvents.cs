using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents
{
	public event Action OnPlayerVisionEnter;
	public event Action OnPlayerVisionExit;

	public void PlayerVisionEnter(bool exitFirst = true)
	{
		if (exitFirst) PlayerVisionExit();
		OnPlayerVisionEnter?.Invoke();
	}
	public void PlayerVisionExit()
	{
		OnPlayerVisionExit?.Invoke();
	}
}
public class GameEvents : MonoBehaviour
{
	public static GameEvents current;

	#region Variables
	public PlayerEvents Player { get; private set; }
	#endregion

	#region UnityFunctions
	private void Awake()
	{
		SetupEventBases();

		if (current != null)
		{
			Debug.LogError("Please make sure there is only one GameEventHandler in the scene!");
			return;
		}
		else
		{
			current = this;
		}
	}

	#endregion

	#region UniqueFunctions
	void SetupEventBases()
	{
		Player = new PlayerEvents();
	}
	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}
