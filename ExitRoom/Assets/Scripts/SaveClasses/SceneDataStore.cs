using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDataStore : MonoBehaviour
{
	public static SceneDataStore current;

	#region Variables
	List<SceneData> data = new List<SceneData>();
	Dictionary<string, int> keys = new Dictionary<string, int>();
	#endregion

	#region UnityFunctions
	public void Awake()
	{
		if (current != null)
		{
			Debug.LogError("Make sure theres only one PuzzleManager in the current scene");
		}
		else
		{
			current = this;
		}

		DontDestroyOnLoad(gameObject);
	}

	#endregion

	#region UniqueFunctions
	public void AddData(SceneData data, string key)
	{
		foreach (string k in keys.Keys)
		{
			if(k == key)
			{
				throw new System.Exception($"Trying to add already existing key: '{key}'!");
			}
		}

		keys.Add(key, this.data.Count);
		this.data.Add(data);
	}
	public SceneData ReadData(string key)
	{
		if (keys.TryGetValue(key, out int targetKey))
		{
			return data[targetKey];
		}

		throw new System.Exception($"Trying to read invalid key: '{key}'!");
	}

	public bool Exists(string key)
	{
		foreach (string k in keys.Keys)
		{
			if (k == key)
			{
				return true;
			}
		}
		return false;
	}
	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}

public class SceneData
{
}