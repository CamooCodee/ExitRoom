using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//Behaviour is on the bush
[RequireComponent(typeof(Interactable))]
public class EnterBushPuzzle : Puzzle
{
	#region Variables
	[Space(20f)]
	public GameObject player;
	public GameObject cam;
	public Transform orientation;

	public Volume postPro;
	ColorAdjustments colorAdjustment;

	public float cameraEndPosHeight = .1f;
	public float cameraEndPosBushOffset = 1.2f;
	public float animationSpeed;
	[Tooltip("Full darkness is ~8")]
	public float targetDarkness = -8f;

	float animationProgress = 0;
	Vector3 camStartPos = Vector3.zero;
	Vector3 camStartRot = Vector3.zero;
	Vector3 camTargetPos = Vector3.zero;
	Vector3 camTargetRot = Vector3.zero;

	bool isAnimating = false;
	#endregion

	#region UnityFunctions
	private void Start()
	{
		postPro.profile.TryGet(out colorAdjustment);
		GetComponent<Interactable>().onInteract += StartAnimate;
	}

	private void Update()
	{
		if (!isActive) return;

		if (isAnimating) AnimateCamera();
	}
	#endregion

	#region UniqueFunctions
	public void StartAnimate()
	{
		if (isAnimating || !isActive) return;

		UIManager.current.DisableAll();

		player.GetComponent<PlayerMovement>().SetActive(false);
		player.GetComponent<PlayerLook>().SetActive(false);

		camStartPos = cam.transform.position;
		camStartRot = cam.transform.localEulerAngles;
		isAnimating = true;
	}
	void AnimateCamera()
	{
		if(camTargetPos == Vector3.zero)
		{
			SetCamTargetPos();
		}
		if (camTargetRot == Vector3.zero)
		{
			SetCamTargetRot();
		}

		Vector3 animationFramePosition = Vector3.Lerp(camStartPos, camTargetPos, animationProgress);
		Vector3 animationFrameRotation = Vector3.Lerp(camStartRot, camTargetRot, animationProgress);
		float animationFrameDarkness = Mathf.Lerp(0f, targetDarkness, animationProgress);

		cam.transform.position = animationFramePosition;
		cam.transform.localEulerAngles = animationFrameRotation;
		colorAdjustment.postExposure.value = animationFrameDarkness;

		if (animationProgress >= 1f)
		{
			isAnimating = false;
			CheckForCompletion();
			return;
		}

		animationProgress += Time.deltaTime * animationSpeed;
	}

	void SetCamTargetPos()
	{
		Vector3 endPos = transform.position;
		endPos.y = cameraEndPosHeight;
		Vector3 toPlayer = transform.position - cam.transform.position;
		float radiusPercentage = Mathf.Clamp01(cameraEndPosBushOffset / toPlayer.magnitude);
		Vector2 offsetXZ = new Vector2((transform.position.x - cam.transform.position.x) * radiusPercentage, (transform.position.z - cam.transform.position.z) * radiusPercentage);

		endPos = new Vector3(transform.position.x - offsetXZ.x, cameraEndPosHeight, transform.position.z - offsetXZ.y);
		camTargetPos = endPos;
	}
	void SetCamTargetRot()
	{
		Vector3 dir3 = camTargetPos - transform.position;
		float yRot = Mathf.Atan2(dir3.x, dir3.z) * Mathf.Rad2Deg + 180f;

		camTargetRot.y = yRot;
	}

	public override void CheckForCompletion()
	{
		if (!isAnimating)
		{
			InvokeOnComplete();
		}

		if (puzzleData.loadNewSceneOnComplete) SceneManager.LoadScene(puzzleData.sceneIndex);
	}
	protected override void DisablePuzzle()
	{
		gameObject.layer = 0;
		Destroy(this);
		Destroy(GetComponent<Interactable>());
	}
	#endregion

	#region DebugFunctions
	#endregion

	#region MathOrLibaryRework

	#endregion
}
