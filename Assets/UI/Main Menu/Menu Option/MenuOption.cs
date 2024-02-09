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
		base.ApplyAnimations(value);

		arrowImage.color = image.color;

		textTransform.anchoredPosition = new(
			Mathf.Lerp(initialTextOffset, selectedOffset, value), 0f);
	}
}
