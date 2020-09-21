using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainPuzzle : MonoBehaviour
{
	#region Variables

	#endregion

	#region UnityFunctions
	private void Awake()
	{
		GetComponent<Interactable>().onInteract += LoadMainScene;
	}
	#endregion

	#region UniqueFunctions
	void LoadMainScene()
	{
		SceneManager.LoadScene(0);
	}
	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}
