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
		TryLoadSceneData();
		ContinueToNextPuzzle();
		CheckForLoadScenePuzzle();
	}
	#endregion

	#region UniqueFunctions
	void ContinueToNextPuzzle()
	{
		if (puzzles[currentPuzzle + 1].puzzleData.loadNewSceneOnComplete)
		{
			puzzles.Add(new LoadScenePuzzle());
		}

		OnNewPuzzle(puzzles[currentPuzzle + 1]);

		try
		{
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

	void CheckForLoadScenePuzzle()
	{
		if (puzzles[currentPuzzle].GetType() == typeof(LoadScenePuzzle))
		{
			puzzles[currentPuzzle].CheckForCompletion();
		}
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
	#endregion
	
	#region DebugFunctions
	
	#endregion
	
	#region MathOrLibaryRework
	
	#endregion
}

public class LoadScenePuzzle : Puzzle
{
	public override void CheckForCompletion()
	{
		InvokeOnComplete();
	}
}

public class PuzzleManagerSceneData : SceneData
{
	public int currentPuzzle = 0;

	public PuzzleManagerSceneData(int puzzle)
	{
		currentPuzzle = puzzle;
	}
}