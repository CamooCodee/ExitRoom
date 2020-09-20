using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
	public static PuzzleManager current;

	#region Variables
	public List<PuzzleCompletor> puzzles;
	[HideInInspector]public int currentPuzzle = 0;

	public delegate void NewPuzzleDelegate(PuzzleCompletor puzzle);
	public event NewPuzzleDelegate OnNewPuzzle;
	#endregion

	#region UnityFunctions
	private void Awake()
	{
		if(current != null)
		{
			Debug.LogError("Make sure theres only one PuzzleManager in the current scene");
		}
		else
		{
			current = this;
		}
	}
	private void Start()
	{
		currentPuzzle = -1;
		ContinueToNextPuzzle();
	}
	#endregion

	#region UniqueFunctions
	void ContinueToNextPuzzle()
	{
		try
		{
			OnNewPuzzle(puzzles[currentPuzzle + 1]);
			puzzles[currentPuzzle].OnComplete -= ContinueToNextPuzzle;
			puzzles[currentPuzzle].Activate(false);
		}
		catch
		{

		}

		currentPuzzle++;

		if (currentPuzzle == puzzles.Count)
		{
			Debug.Log("Played Through");
			return;
		}

		puzzles[currentPuzzle].OnComplete += ContinueToNextPuzzle;
		puzzles[currentPuzzle].Activate(true);
	}
	#endregion
	
	#region DebugFunctions
	
	#endregion
	
	#region MathOrLibaryRework
	
	#endregion
}
