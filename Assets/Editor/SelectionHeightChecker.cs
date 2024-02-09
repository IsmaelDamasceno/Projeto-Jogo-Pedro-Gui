using UnityEngine;
using UnityEditor;

public class SelectionHeightChecker : EditorWindow
{
	[MenuItem("Tools/Check Selection Height")]
	private static void CheckSelectionHeight()
	{
		// Get the currently selected objects
		GameObject[] selectedObjects = Selection.gameObjects;

		if (selectedObjects.Length == 0)
		{
			Debug.LogWarning("No objects selected!");
			return;
		}

		// Calculate the total height of the selection
		float minY = float.MaxValue;
		float maxY = float.MinValue;
		foreach (GameObject obj in selectedObjects)
		{
			RectTransform rectTransform = obj.GetComponent<RectTransform>();
			if (rectTransform != null)
			{
				// y + h * p
				float positionMaxY =
					rectTransform.localPosition.y + rectTransform.sizeDelta.y * rectTransform.localScale.y * rectTransform.pivot.y;

				// y - h * p
				float positionMinY =
					rectTransform.localPosition.y - rectTransform.sizeDelta.y * rectTransform.localScale.y * rectTransform.pivot.y;

				if (positionMaxY > maxY)
				{
					maxY = positionMaxY;
				}
				if (positionMinY < minY)
				{
					minY = positionMinY;
				}
			}
		}
		float center = (maxY + minY) / 2f;

		foreach (GameObject obj in selectedObjects)
		{
			RectTransform rectTransform = obj.GetComponent<RectTransform>();
			if (rectTransform != null)
			{
				rectTransform.localPosition += Vector3.down * center;
			}
		}
	}
}