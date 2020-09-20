using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
	#region Variables
	public bool disableOnEndState = true;
	public float animationSpeed;
	public LeanTweenType easeType = LeanTweenType.linear;
	public AnimationCurve animCurve;
	[Space(12f)]
	public UIAnimation[] connections;
	[Space(12f)]
	public Vector3 startPos;
	public Vector3 endPos;
	[Space(5f)]
	public Color startColor;
	public Color endColor;
	#endregion

	#region UnityFunctions

	#endregion

	#region UniqueFunctions
	void Progress()
	{
		if(startPos != endPos)LeanTween.moveLocal(gameObject, endPos, 0.1f * animationSpeed).setEase(easeType);
		if(startColor != endColor)LeanTween.color(gameObject, endColor, 0.1f * animationSpeed).setEase(easeType);
	}
	void ReverseProgress()
	{
		Action onComplete = delegate { gameObject.SetActive(!disableOnEndState); };

		if (startPos != endPos) LeanTween.moveLocal(gameObject, startPos, 0.1f * animationSpeed)
				.setEase(easeType)
				.setOnComplete(onComplete);

		if (startColor != endColor) LeanTween.color(gameObject, startColor, 0.1f * animationSpeed)
				.setEase(easeType)
				.setOnComplete(onComplete);
	}

	public void StartAnimation()
	{
		if (!LeanTween.isTweening(gameObject))
		{
			transform.localPosition = startPos;
			transform.GetComponent<Graphic>().color = startColor;

			foreach (UIAnimation connection in connections)
			{
				connection.StartAnimation();
			}

			Progress();
		}
	}
	public void StartReverseAnimation()
	{
		if (!LeanTween.isTweening(gameObject))
		{
			transform.localPosition = endPos;
			transform.GetComponent<Graphic>().color = endColor;

			foreach (UIAnimation connection in connections)
			{
				connection.StartReverseAnimation();
			}

			ReverseProgress();
		}
	}
	public void SetActiveWithAnimation(bool state)
	{
		if (state)
		{
			StartAnimation();
		}
		else
		{
			StartReverseAnimation();
		}
	}
	#endregion

	#region DebugFunctions

	#endregion

	#region MathOrLibaryRework

	#endregion
}
