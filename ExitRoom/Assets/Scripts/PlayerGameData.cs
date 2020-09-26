using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameData : MonoBehaviour
{
	#region Variables
	int currentKeys = -1;
	public event Action<int> onNewKey; 
	#endregion

	#region UnityFunctions
	private void Awake()
	{
		if (SceneDataStore.current.Exists("player"))
		{
			PlayerSceneData loadedData = SceneDataStore.current.ReadData("player") as PlayerSceneData;
			currentKeys = loadedData.keys;
			onNewKey?.Invoke(currentKeys);
		}

		PuzzleManager.current.OnNewPuzzle += FoundKey;
	}
	#endregion

	#region UniqueFunctions
	public void FoundKey(Puzzle puzzle)
	{
		if (puzzle == null) return;

		currentKeys++;
		SceneDataStore.current.AddData(new PlayerSceneData(currentKeys), "player");
		onNewKey?.Invoke(currentKeys);
	}
	public int GetKeyAmount() 
	{ 
		return currentKeys;
	}
	#endregion
	
	#region DebugFunctions
	
	#endregion
	
	#region MathOrLibaryRework
	
	#endregion
}

public class PlayerSceneData : SceneData
{
	public int keys = 0;

	public PlayerSceneData(int keyAmount)
	{
		keys = keyAmount;
	}
}