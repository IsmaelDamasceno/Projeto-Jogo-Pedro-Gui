using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class MenuOption : ToggleButton
{
	[Header("Offset")]
	[SerializeField] private float selectedOffset;

	private float initialTextOffset;
	private RectTransform textTransform;
	private Image arrowImage;

	private void Start()
	{
		Init();

		arrowImage = Utils.SearchObjectWithComponent<Image>(transform, "Arrow");
		textTransform = text.GetComponent<RectTransform>();
		initialTextOffset = textTransform.anchoredPosition.x;

		arrowImage.color = image.color;
	}

	protected override void ApplyAnimations(float value)
	{
		text.color = new(
			Mathf.Lerp(unselectedColor.r, selectedColor.r, value),
			Mathf.Lerp(unselectedColor.g, selectedColor.g, value),
			Mathf.Lerp(unselectedColor.b, selectedColor.b, value),
			1f
		);

		image.color = new(
			Mathf.Lerp(unselectedBackgroundColor.r, selectedBackgroundColor.r, value),
			Mathf.Lerp(unselectedBackgroundColor.g, selectedBackgroundColor.g, value),
			Mathf.Lerp(unselectedBackgroundColor.b, selectedBackgroundColor.b, value),
			Mathf.Lerp(unselectedBackgroundColor.a, selectedBackgroundColor.a, value)
		);
		arrowImage.color = image.color;

		textTransform.anchoredPosition = new(
			Mathf.Lerp(initialTextOffset, selectedOffset, value), 0f);
	}
}
