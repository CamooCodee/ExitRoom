using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadingHanler : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}
