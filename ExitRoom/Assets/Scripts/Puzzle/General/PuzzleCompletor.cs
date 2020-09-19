﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCompletor : MonoBehaviour
{
	#region Variables
	public PuzzleData puzzleData;

	public delegate void EmptyCompleteDelegate();
	public event EmptyCompleteDelegate OnComplete;

	public bool isActive { get; private set; } = false;
	#endregion
	
	#region UniqueFunctions
	public void Activate(bool state)
	{
		isActive = state;
	}

	public virtual void CheckForCompletion()
	{

	}

	protected void InvokeOnComplete()
	{
		OnComplete.Invoke();
	}
	#endregion
}