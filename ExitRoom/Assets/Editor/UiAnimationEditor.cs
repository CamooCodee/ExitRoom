using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(UIAnimation))]
public class UiAnimationEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		GUILayout.Space(10f);

		UIAnimation myTarget = (UIAnimation)target;

		if (GUILayout.Button("Add Start-Position"))
		{
			myTarget.startPos = myTarget.transform.localPosition;
		}
		if (GUILayout.Button("Add End-Position"))
		{
			myTarget.endPos = myTarget.transform.localPosition;
		}
		if (GUILayout.Button("Add Start-Color"))
		{
			if(myTarget.GetComponent<Graphic>() != null) myTarget.startColor = myTarget.GetComponent<Graphic>().color;
		}
		if (GUILayout.Button("Add End-Color"))
		{
			if (myTarget.GetComponent<Graphic>() != null) myTarget.endColor = myTarget.GetComponent<Graphic>().color;
		}
	}
}
