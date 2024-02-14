using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SliderOption : ToggleButton
{

	[SerializeField] private float slideSpeed;
	[SerializeField] float currentSliderValue;
	[SerializeField] private VolumeType volumeType;

	protected new UnityEvent onClickEvent;

	private float maxWidth;
	private float currentPushValue;

	private RectTransform sliderValueTrs;
	private RectTransform sliderIconTrs;

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
