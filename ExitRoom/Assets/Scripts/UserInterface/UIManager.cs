using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class UIManager : MonoBehaviour
{
	public static UIManager current;

	#region Variables
	[HideInInspector]public GameObject openedUIOverlay = null;
	[HideInInspector]public bool closeUIOverlay;

	public string TextPopUpAnim_PopUpStateName;
	public KeyCode[] closeUIOverlayKeys;
	[Space(18f)]
	public GameObject canvas;
	public List<UIBase> uiManager = new List<UIBase>();

	#region BookUI
	public Component ui_Book;
	public TextMeshProUGUI ui_BookHintText;
	#endregion
	[Space(10f)]
	#region VisionUI
	public Component ui_VisionInfo;
	#endregion
	[Space(10f)]
	#region TreasurePin
	public Component ui_PinInputMain;
	public TMP_InputField ui_PinInput;
	public Button ui_TryPinButton;
	#endregion
	[Space(10f)]
	#region PlayerWarning
	public TextMeshProUGUI ui_InactiveTaskDisplay;
	#endregion
	#region PlayerInformation
	public TextMeshProUGUI ui_KeyDisplay;
	public Component ui_FinalOverlay;
	#endregion
	[Space(10f)]

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
		AddAllManagers();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F5))
		{
			if (canvas.activeSelf)
			{
				DisableAll();
			}
			else
			{
				EnableAll();
			}
		}

		foreach (KeyCode key in closeUIOverlayKeys)
		{
		    if(Input.GetKeyDown(key))
			{
				closeUIOverlay = true;
				break;
			}
			else
			{
				closeUIOverlay = false;
			}
		}

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
	void AddAllManagers()
	{
		if (!BookUI.isSceneRestricted || GameManager.IsInMainScene()) AddManager(new BookUI(current, ui_Book, ui_BookHintText));
		if (!VisionUI.isSceneRestricted || GameManager.IsInMainScene()) AddManager(new VisionUI(current, ui_VisionInfo));
		if (!TreasurePinUI.isSceneRestricted || GameManager.IsInMainScene()) AddManager(new TreasurePinUI(current, ui_PinInputMain, ui_PinInput, ui_TryPinButton));
		if (!PlayerWarningsUI.isSceneRestricted || GameManager.IsInMainScene()) AddManager(new PlayerWarningsUI(current, ui_InactiveTaskDisplay));
		if (!PlayerInformationUI.isSceneRestricted || GameManager.IsInMainScene()) AddManager(new PlayerInformationUI(current, ui_KeyDisplay, ui_FinalOverlay));
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
			if (obj != apartFrom && !obj.IsIndirectChildOf(apartFrom.transform) && !obj.transform.CompareTag("ActivationLock")
				&& !obj.CompareTag("UIOverlay"))
			{
				obj.gameObject.SetActive(true);
			}
		}
	}

	public void CallFunctionOnAllManagers(string functionName, object[] parameters)
	{
		foreach (UIBase manager in uiManager)
		{
			MethodInfo method = manager.GetType().GetMethod(functionName);
			if(method != null)
			{
				method.Invoke(manager, parameters);
			}
		}
	}
	public T GetManager<T>()
	{
		foreach (UIBase manager in uiManager)
		{
			if(manager.GetType() == typeof(T))
			{
				return (T)(object)manager;
			}
		}

		throw new Exception($"Couldn't find a Manager with the given Type: {typeof(T).Name}");
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

//TODO Warn if UiBase child doesnt have a static isSceneResticted field
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
	public static bool isSceneRestricted = false;
	bool bookActiveState = false;
	readonly GameObject book;
	readonly TextMeshProUGUI bookText;

	public BookUI(UIManager manager, Component bookObj, TextMeshProUGUI bookHintText) : base(manager)
	{
		book = bookObj.gameObject;
		bookText = bookHintText;

		bookActiveState = book.activeSelf;

		if(PuzzleManager.current != null)PuzzleManager.current.OnNewPuzzle += UpdateBookHint;
	}

	#region UIBaseFuncs
	public override void InputThread()
	{
		if(Manager.closeUIOverlay && Manager.openedUIOverlay == book)
		{
			ToggleBook();
			return;
		}
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
		if (LeanTween.isTweening(book) || (Manager.openedUIOverlay != book && Manager.openedUIOverlay != null)) return;

		bookActiveState = !book.activeSelf;
		book.SetActiveWithAnimation(!book.activeSelf);
		GameManager.current.ActivatePlayerControlls(!bookActiveState);
		GameManager.SetCursorLockState(!bookActiveState);

		if (bookActiveState)
		{
			Manager.DisableAll(book.transform);
			Manager.openedUIOverlay = book;
		}
		else
		{
			Manager.openedUIOverlay = null;
			Manager.EnableAll(book.transform);
			Manager.HandleMajorUiRedraw();
		}
	}

	void UpdateBookHint(Puzzle currentPuzzle)
	{
		if (currentPuzzle.puzzleData == null) return;
		
		bookText.text = currentPuzzle.puzzleData.bookHint;
	}
}
public class VisionUI : UIBase
{
	public static bool isSceneRestricted = false;
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
public class TreasurePinUI : UIBase
{
	public static bool isSceneRestricted = true;
	readonly GameObject pinObject;
	readonly TMP_InputField pinInput;
	readonly Button tryPinButton;
	public event Action<string> onPinEnter;

	public TreasurePinUI(UIManager manager, Component pinInputParent, TMP_InputField pinDisp, Button tryButton) : base(manager)
	{
		pinObject = pinInputParent.gameObject;
		pinInput = pinDisp;
		tryPinButton = tryButton;
		InitializeButton();

		pinInput.onSubmit.AddListener(TryPin);
		pinInput.onSubmit.AddListener(ClearPin);
		pinInput.onSelect.AddListener(ClearPin);
	}

	#region UIBaseFuncs
	public override void InputThread()
	{
		
	}
	public override void UpdateThread()
	{
		if(pinInput.isFocused && pinInput.characterLimit == pinInput.text.Length)
		{
			pinInput.OnDeselect(new BaseEventData(EventSystem.current));
		}
		if(Manager.closeUIOverlay && Manager.openedUIOverlay == pinObject)
		{
			TogglePinInput();
		}
	}
	#endregion
	public void ClearPin(string input)
	{
		pinInput.text = "";
	}

	void InitializeButton()
	{
		tryPinButton.onClick.AddListener(TryPin);
	}

	public void TogglePinInput()
	{
		if (Manager.openedUIOverlay != pinObject && Manager.openedUIOverlay != null) return;

		pinObject.SetActive(!pinObject.activeSelf);

		GameManager.current.ActivatePlayerControlls(!pinObject.activeSelf);
		GameManager.SetCursorLockState(!pinObject.activeSelf);

		if (pinObject.activeSelf)
		{
			Manager.openedUIOverlay = pinObject;
			Manager.DisableAll(pinObject.transform);
		}
		else
		{
			Manager.openedUIOverlay = null;
			Manager.EnableAll(pinObject.transform);
			Manager.HandleMajorUiRedraw();
		}
	}

	public void TryPin(string typedPin)
	{
		string pin = typedPin;

		onPinEnter?.Invoke(pin);
		TogglePinInput();
	}
	public void TryPin()
	{
		string pin = pinInput.text;

		onPinEnter?.Invoke(pin);
		TogglePinInput();
	}
}
public class PlayerWarningsUI : UIBase
{
	public static bool isSceneRestricted = true;
	public TextMeshProUGUI inactivePuzzleDisplay;

	public PlayerWarningsUI(UIManager manager, TextMeshProUGUI inactivePuzzleDisp) : base(manager)
	{
		inactivePuzzleDisplay = inactivePuzzleDisp;
	}

	public void ThrowInactivePuzzleWarning()
	{
		Animator displayAnim = inactivePuzzleDisplay.GetComponent<Animator>();
		if (displayAnim == null)
		{
			throw new Exception("The inactivePuzzleDisplay-UiObject is expected to have an Animator");
		}

		if (!displayAnim.GetCurrentAnimatorStateInfo(0).IsTag(Manager.TextPopUpAnim_PopUpStateName))
		{
			displayAnim.SetTrigger("Activate");
		}
	}
}
public class PlayerInformationUI : UIBase
{
	public static bool isSceneRestricted = true;
	public TextMeshProUGUI keyDisplay;
	public GameObject finalScreen;

	public PlayerInformationUI(UIManager manager, TextMeshProUGUI keyDisp, Component finalOverlay) : base(manager)
	{
		keyDisplay = keyDisp;
		GameManager.GetPlayerComponent<PlayerGameData>().onNewKey += UpdateKeyDisplay;
		finalScreen = finalOverlay.gameObject;
	}

	void UpdateKeyDisplay(int keyAmoumnt)
	{
		keyDisplay.text = keyAmoumnt.ToString();

		if (GameManager.GetPlayerComponent<PlayerGameData>().GetKeyAmount() >= 2) EnableFinalScreen();
	}

	void EnableFinalScreen()
	{
		finalScreen.SetActive(true);

		Manager.DisableAll(finalScreen.transform);
		Manager.openedUIOverlay = finalScreen;
		Time.timeScale = 0f;
	}
}
#endregion
