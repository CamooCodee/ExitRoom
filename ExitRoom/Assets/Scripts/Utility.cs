using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
	public static bool IsChildOf(this Component target, Transform parent)
	{
		return target.transform.parent == parent;
	}
	public static bool IsIndirectChildOf(this Component target, Transform parent)
	{
		//Can be optimized by checking if the parents left is equal to the 'parent' parents left

		Transform currentlyInspecting = target.transform.parent;

		while (currentlyInspecting.transform.parent != null)
		{
			if(currentlyInspecting == parent)
			{
				return true;
			}

			currentlyInspecting = currentlyInspecting.parent;
		}

		return false;
	}
}
