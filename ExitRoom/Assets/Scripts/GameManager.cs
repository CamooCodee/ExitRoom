﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager current;

	#region Variables
	public GameObject player;
	#endregion

	#region UnityFunctions
	private void Awake()
	{
		if (current != null)
		{
			Debug.LogError("Make sure theres only one GameManager in the current scene");
		}
		else
		{
			current = this;
		}
	}
	#endregion

	#region UniqueFunctions


	public static void SetCursorLockState(bool lockedState)
	{
		if (!lockedState)
		{
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	public void ActivatePlayerControlls(bool state)
	{
		player.GetComponent<PlayerLook>().SetActive(state);
		player.GetComponent<PlayerMovement>().SetActive(state);
	}
	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}