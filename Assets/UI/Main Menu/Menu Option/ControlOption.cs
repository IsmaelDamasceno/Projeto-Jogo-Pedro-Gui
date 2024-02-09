using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ControlOption : ToggleButton
{

	[Header("Binding")]
	[SerializeField] private string action;

	private TextMeshProUGUI labelText;
	private TextMeshProUGUI keyboardText;
	private Image arrowImage;

	protected new UnityEvent onClickEvent;

	void Start()
    {
		Init();

		RectTransform contentTrs =
			Utils.SearchObjectWithComponent<RectTransform>(transform, "Content");

		labelText = Utils.SearchObjectWithComponent<TextMeshProUGUI>(contentTrs, "Label");
		keyboardText = Utils.SearchObjectWithComponent<TextMeshProUGUI>(contentTrs, "Keyboard Button");
		
		arrowImage = Utils.SearchObjectWithComponent<Image>(transform, "Arrow");
		arrowImage.color = image.color;
	}
	protected override void ApplyAnimations(float value)
	{
		labelText.color = new(
			Mathf.Lerp(unselectedColor.r, selectedColor.r, value),
			Mathf.Lerp(unselectedColor.g, selectedColor.g, value),
			Mathf.Lerp(unselectedColor.b, selectedColor.b, value),
			1f
		);
		keyboardText.color = labelText.color;

		image.color = new(
			Mathf.Lerp(unselectedBackgroundColor.r, selectedBackgroundColor.r, value),
			Mathf.Lerp(unselectedBackgroundColor.g, selectedBackgroundColor.g, value),
			Mathf.Lerp(unselectedBackgroundColor.b, selectedBackgroundColor.b, value),
			Mathf.Lerp(unselectedBackgroundColor.a, selectedBackgroundColor.a, value)
		);
		arrowImage.color = image.color;
	}
	public override void Interact()
	{
		if (!transform.parent.parent.gameObject.activeInHierarchy)
		{
			return;
		}
		Rebind();
	}

	public void Rebind()
	{
		InputRebind.RebindAction(action);
	}
}
