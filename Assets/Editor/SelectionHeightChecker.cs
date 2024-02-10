using UnityEngine;
using UnityEditor;

public class SelectionHeightChecker : EditorWindow
{
	[MenuItem("Tools/Average Selection Height")]
	private static void AverageSelectionHeight()
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

	[MenuItem("Tools/Apply equal spacing")]
	private static void ApplyEqualSpacing()
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
		float height = maxY - minY;
		float increment = height / selectedObjects.Length;
		for (int i = 0; i < selectedObjects.Length; i++)
		{
			RectTransform rectTransform = selectedObjects[i].GetComponent<RectTransform>();
			if (rectTransform != null)
			{
				rectTransform.localPosition = new(
					rectTransform.localPosition.x,
					(selectedObjects.Length - i) * increment,
					rectTransform.localPosition.z
				);
			}
		}
	}
}