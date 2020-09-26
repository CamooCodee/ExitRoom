using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	private void Start()
	{
		//SetCursorLockState(false);
		//ActivatePlayerControlls(false);
	}

	private void Update()
	{
		if(Cursor.lockState == CursorLockMode.Locked && Cursor.visible)
		{
			Cursor.visible = false;
		}
		else if(!Cursor.visible && Cursor.lockState == CursorLockMode.None)
		{
			Cursor.visible = true;
		}
	}
	#endregion

	#region UniqueFunctions


	public static void SetCursorLockState(bool lockedState)
	{
		if (!lockedState)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	public void ActivatePlayerControlls(bool state)
	{
		player.GetComponent<PlayerLook>().SetActive(state);
		player.GetComponent<PlayerMovement>().SetActive(state);
	}

	public static bool IsInMainScene()
	{
		return SceneManager.GetActiveScene().buildIndex == 0;
	}

	public static T GetPlayerComponent<T>()
	{
		return current.player.GetComponent<T>();
	}
	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}
