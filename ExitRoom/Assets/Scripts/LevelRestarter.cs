﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRestarter : MonoBehaviour
{
	#region Variables
	public GameObject player;
	Vector3 startPos;
	#endregion

	#region UnityFunctions
	private void Start()
	{
		startPos = player.transform.position;
	}

	private void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			Restart();
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player")) Restart();
	}
	#endregion

	#region UniqueFunctions
	void Restart()
	{
		player.transform.position = startPos;
	}
	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}
