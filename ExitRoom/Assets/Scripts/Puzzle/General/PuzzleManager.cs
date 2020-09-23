using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
	const string sceneDataStoreKey = "PuzzleManager";

	public static PuzzleManager current;

	#region Variables
	public List<Puzzle> puzzles;
	[HideInInspector]public int currentPuzzle = 0;

	public delegate void NewPuzzleDelegate(Puzzle puzzle);
	public event NewPuzzleDelegate OnNewPuzzle;
	#endregion

	#region UnityFunctions
	private void Awake()
	{
		if(current != null && current.gameObject != null)
		{
			Debug.LogError("Make sure theres only one PuzzleManager in the current scene");
		}
		else
		{
			current = this;
		}
		currentPuzzle = -1;
		TryLoadSceneData();
		DeactivateFinishedPuzzles();
	}
	private void Start()
	{
		ContinueToNextPuzzle();
	}
	#endregion

	#region UniqueFunctions
	void ContinueToNextPuzzle()
	{
		try
		{
			puzzles[currentPuzzle].OnComplete -= ContinueToNextPuzzle;
			puzzles[currentPuzzle].Activate(false);
		}
		catch { }

		currentPuzzle++;

		if (currentPuzzle == puzzles.Count)
		{
			Debug.Log("Played Through");
			return;
		}

		if (puzzles[currentPuzzle] == null)
		{
			WriteSceneData();
			return;
		}
		if (puzzles[currentPuzzle].puzzleData.loadNewSceneOnComplete)
		{
			puzzles.Insert(currentPuzzle + 1, null);
		}

		OnNewPuzzle(puzzles[currentPuzzle]);


		puzzles[currentPuzzle].OnComplete += ContinueToNextPuzzle;
		puzzles[currentPuzzle].Activate(true);
	}

	void TryLoadSceneData()
	{
		if (SceneDataStore.current.Exists(sceneDataStoreKey))
		{
			LoadSceneData();
		}
	}

	void WriteSceneData()
	{
		SceneDataStore.current.AddData(new PuzzleManagerSceneData(currentPuzzle), sceneDataStoreKey);
	}
	void LoadSceneData()
	{
		PuzzleManagerSceneData loadedData = SceneDataStore.current.ReadData(sceneDataStoreKey) as PuzzleManagerSceneData;

		currentPuzzle = loadedData.currentPuzzle;
	}

	void DeactivateFinishedPuzzles()
	{
		for (int i = 0; i < currentPuzzle; i++)
		{
			if (puzzles[i].puzzleData.loadNewSceneOnComplete)
			{
				puzzles.Insert(i + 1, null);
			}

			puzzles[i].Activate(false);
		}
	}
	#endregion
	
	#region DebugFunctions
	
	#endregion
	
	#region MathOrLibaryRework
	
	#endregion
}

public class PuzzleManagerSceneData : SceneData
{
	public int currentPuzzle = 0;

	public PuzzleManagerSceneData(int puzzle)
	{
		currentPuzzle = puzzle;
	}
}