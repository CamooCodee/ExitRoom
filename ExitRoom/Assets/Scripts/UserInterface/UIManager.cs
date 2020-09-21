using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
	public static UIManager current;

	#region Variables
	public GameObject canvas;
	public List<UIBase> uiManager = new List<UIBase>();

	#region BookUI
	public Component ui_Book;
	public TextMeshProUGUI ui_BookHintText;
	#endregion
	#region VisionUI
	public Component ui_VisionInfo;
	#endregion

	public Component ui_Crosshair;

	public List<Component> uiObjects = new List<Component>();

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
		InitializeUIObjects();
		AddManager(new BookUI(current, ui_Book, ui_BookHintText));
		AddManager(new VisionUI(current, ui_VisionInfo));
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
	void InitializeUIObjects()
	{
		FieldInfo[] fields = this.GetType().GetFields();

		foreach (FieldInfo field in fields)
		{
			if (field.Name.Contains("ui_"))
			{
				if(!field.FieldType.IsSubclassOf(typeof(Component)) && field.FieldType != typeof(Component))
				{
					throw new System.Exception($"Variables tagged with 'ui_' can't be of type: {field.FieldType.Name}. Needs to be a Component");
				}
				uiObjects.Add(field.GetValue(this) as Component);
			}
		}
	}

	public void DisableAll(Component apartFrom = null)
	{
		if (apartFrom == null)
		{
			canvas.SetActive(false);
			return;
		}

		foreach (Component obj in uiObjects)
		{
			if(obj != apartFrom && !obj.IsIndirectChildOf(apartFrom.transform))
			{
				obj.gameObject.SetActive(false);
			}
		}
	}
	public void EnableAll(Component apartFrom = null)
	{
		if (!canvas.activeSelf || apartFrom == null)
		{
			canvas.SetActive(true);
			return;
		}

		foreach (Component obj in uiObjects)
		{
			if (obj != apartFrom && !obj.IsIndirectChildOf(apartFrom.transform) && !obj.transform.CompareTag("ActivationLock"))
			{
				obj.gameObject.SetActive(true);
			}
		}
	}

	public void HandleMajorUiRedraw()
	{
		GameEvents.current.RecallAllEvents();
	}

	public void AddManager(UIBase manager)
	{
		uiManager.Add(manager);
	}
	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}

public class UIBase
{
	public readonly UIManager Manager;

	public UIBase(UIManager manager)
	{
		Manager = manager;
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
	bool bookActiveState = false;
	readonly GameObject book;
	readonly TextMeshProUGUI bookText;

	public BookUI(UIManager manager, Component bookObj, TextMeshProUGUI bookHintText) : base(manager)
	{
		book = bookObj.gameObject;
		bookText = bookHintText;

		bookActiveState = book.activeSelf;

		PuzzleManager.current.OnNewPuzzle += UpdateBookHint;
	}

	#region UIBaseFuncs
	public override void InputThread()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			ToggleBook();
		}
	}
	public override void UpdateThread()
	{

	}
	#endregion

	void ToggleBook()
	{
		bookActiveState = !book.activeSelf;
		book.SetActiveWithAnimation(!book.activeSelf);
		GameManager.current.ActivatePlayerControlls(!bookActiveState);
		//GameManager.SetCursorLockState(!bookActiveState);

		if (bookActiveState)
		{
			Manager.DisableAll(book.transform);
		}
		else
		{
			Manager.EnableAll(book.transform);
			Manager.HandleMajorUiRedraw();
		}
	}
	void UpdateBookHint(PuzzleCompletor currentPuzzle)
	{
		if (currentPuzzle.puzzleData == null) return;

		bookText.text = currentPuzzle.puzzleData.bookHint;
	}
}
public class VisionUI : UIBase
{
	readonly GameObject visionInfo;

	public VisionUI(UIManager manager, Component visionPopUp) : base(manager)
	{
		visionInfo = visionPopUp.gameObject;

		GameEvents.current.Player.OnPlayerVisionEnter += ActivatePopUp;
		GameEvents.current.Player.OnPlayerVisionExit += DeactivatePopUp;
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
