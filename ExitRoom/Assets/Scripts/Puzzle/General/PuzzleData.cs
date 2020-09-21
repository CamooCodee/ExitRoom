using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPuzzle", menuName = "Puzzle")]
public class PuzzleData : ScriptableObject
{
	public bool loadNewSceneOnComplete;
	public int sceneIndex;

	[TextArea(5, 10)]
	public string bookHint = "";
	[TextArea(3, 6)]
	public string hint1 = "";
	[TextArea(3, 6)]
	public string hint2 = "";
}
