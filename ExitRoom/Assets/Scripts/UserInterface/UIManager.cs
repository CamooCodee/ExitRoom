using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIBase
{
	public GameObject player;

	public UIBase(GameObject player)
	{
		this.player = player;
	}

	public virtual void InputThread()
	{

	}
	public virtual void UpdateThread()
	{

	}
}
#region UIClasses
public class BookUI : UIBase
{
	GameObject book;
	TextMeshProUGUI bookText;

	public BookUI(GameObject bookObj, TextMeshProUGUI bookHintText, GameObject player) : base (player)
	{
		book = bookObj;
		bookText = bookHintText;

		PuzzleManager.current.OnNewPuzzle += UpdateBookHint;
	}

	#region UIBaseFuncs
	public override void InputThread()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			book.SetActive(!book.activeSelf);
		}
	}
	public override void UpdateThread()
	{
		
	}
	#endregion

	void UpdateBookHint(PuzzleCompletor currentPuzzle)
	{
		if(currentPuzzle.puzzleData == null) return;

		bookText.text = currentPuzzle.puzzleData.bookHint;
	}
}

public class VisionUI : UIBase
{
	GameObject visionInfo;

	public VisionUI(GameObject visionPopUp, GameObject player) : base (player)
	{
		visionInfo = visionPopUp;

		player.GetComponent<PlayerVision>().OnVisionEnter += ActivatePopUp;
		player.GetComponent<PlayerVision>().OnVisionExit += DeactivatePopUp;
	}

	#region UIBaseFuncs
	public override void InputThread()
	{
		
	}
	public override void UpdateThread()
	{

	}
	#endregion

	void ActivatePopUp()
	{
		visionInfo.SetActive(true);
	}
	void DeactivatePopUp()
	{
		visionInfo.SetActive(false);
	}
}
#endregion

public class UIManager : MonoBehaviour
{
	public static UIManager current;

	#region Variables
	public GameObject canvas;
	public List<UIBase> uiManager = new List<UIBase>();
	public GameObject player;

	#region BookUI
	public GameObject book;
	public TextMeshProUGUI bookHintText;
	#endregion
	#region VisionUI
	public GameObject visionInfo;
	#endregion

	#endregion

	#region UnityFunctions
	private void Awake()
	{
		if (current != null)
		{
			Debug.LogError("Make sure theres only one UIManager in the current scene");
		}
		else
		{
			current = this;
		}
	}
	private void Start()
	{
		AddManager(new BookUI(book, bookHintText, player));
		AddManager(new VisionUI(visionInfo, player));
	}

	private void Update()
	{
		foreach (UIBase manager in uiManager)
		{
			manager.InputThread();
			manager.UpdateThread();
		}
	}
	#endregion

	#region UniqueFunctions
	public void AddManager(UIBase manager)
	{
		uiManager.Add(manager);
	}
	public void DeactivateAll()
	{
		canvas.SetActive(false);
	}
	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}
