using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderOption : ToggleButton
{
	[SerializeField] private float slideSpeed;
	[SerializeField] float currentSliderValue;
	[SerializeField] private VolumeType volumeType;

	protected new UnityEvent onClickEvent;

	private float minWidth;
	private float maxWidth;
	private float currentPushValue;

	private RectTransform sliderValueTrs;
	private RectTransform sliderIconTrs;
	private bool blockMouse = false;

	private void OnEnable()
	{
		if (Input.GetMouseButton(0))
		{
			blockMouse = true;
		}

		SetSliderValue(SoundVolumeController.GetVolume(volumeType));
	}

	protected override void Init()
	{
		image = GetComponent<Image>();
		text = Utils.SearchObjectWithComponent<TextMeshProUGUI>(transform, "Label");

		if (text != null)
		{
			text.color = unselectedColor;
		}
		image.color = unselectedBackgroundColor;

		GameObject sliderParent = Utils.SearchObjectIntransform(transform, "Slider Parent");
		sliderValueTrs = Utils.SearchObjectWithComponent<RectTransform>(sliderParent.transform, "Slider Full");
		sliderIconTrs = Utils.SearchObjectWithComponent<RectTransform>(sliderParent.transform, "Slider Icon");

		minWidth = sliderValueTrs.anchoredPosition.x;
		maxWidth = sliderParent.GetComponent<RectTransform>().sizeDelta.x;

		float newVisualValue = currentSliderValue * maxWidth;
		sliderValueTrs.sizeDelta = new(newVisualValue, sliderValueTrs.sizeDelta.y);
		sliderIconTrs.anchoredPosition = new(
			newVisualValue, sliderIconTrs.anchoredPosition.y
		);

		InputListener.navigateEvent.AddListener(NavigateListener);
	}

	private void NavigateListener(Vector2 inputValue)
	{
		if (selected && direction >= 0)
		{
			currentPushValue = inputValue.x;
		}
	}

	public new void Update()
	{
		base.Update();

		if (!selected)
		{
			currentPushValue = 0f;
		}

		if (currentPushValue != 0f)
		{
			ShiftSliderValue(Time.unscaledDeltaTime * slideSpeed * currentPushValue);
		}

		if (Input.GetMouseButtonUp(0))
		{
			blockMouse = false;
		}

		if (hovered && Input.GetMouseButton(0) && !blockMouse)
		{
			Vector2 mousePosition = Input.mousePosition;

			Canvas canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, mousePosition, canvas.worldCamera, out Vector2 mousePositionUI);

			float visualPosition = Mathf.Clamp(mousePositionUI.x, minWidth, maxWidth);
			float maxValue = maxWidth - minWidth;
			float value = (visualPosition - minWidth) / maxValue;

			 currentSliderValue = value;
			currentSliderValue = Mathf.Clamp(currentSliderValue, 0f, 1f);

			sliderValueTrs.sizeDelta = new(visualPosition, sliderValueTrs.sizeDelta.y);
			sliderIconTrs.anchoredPosition = new(
				visualPosition, sliderIconTrs.anchoredPosition.y
			);

			SoundVolumeController.SetVolume(volumeType, currentSliderValue);
		}
	}

	public void SetSliderValue(float value)
	{
		currentSliderValue = value;
		currentSliderValue = Mathf.Clamp(currentSliderValue, 0f, 1f);

		float newVisualValue = currentSliderValue * maxWidth;
		sliderValueTrs.sizeDelta = new(newVisualValue, sliderValueTrs.sizeDelta.y);
		sliderIconTrs.anchoredPosition = new(
			newVisualValue, sliderIconTrs.anchoredPosition.y
		);

		SoundVolumeController.SetVolume(volumeType, currentSliderValue);
	}
	public void ShiftSliderValue(float value)
	{
		currentSliderValue += value;
		currentSliderValue = Mathf.Clamp(currentSliderValue, 0f, 1f);

		float newVisualValue = currentSliderValue * maxWidth;
		sliderValueTrs.sizeDelta = new(newVisualValue, sliderValueTrs.sizeDelta.y);
		sliderIconTrs.anchoredPosition = new(
			newVisualValue, sliderIconTrs.anchoredPosition.y
		);

		SoundVolumeController.SetVolume(volumeType, currentSliderValue);
	}

	public override void Interact()
	{

	}
}
