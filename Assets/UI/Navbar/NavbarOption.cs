using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class NavbarOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[Header("Color")]
	[SerializeField] protected Color unselectedColor;
	[SerializeField] protected Color selectedColor;

	[SerializeField] protected AnimationCurve selectionCurve;
	[SerializeField] protected float animationTime;

	[SerializeField] protected GameObject menu;

	protected float currentTime = 0f;
	protected int direction = 0;

	protected TextMeshProUGUI text;
	[HideInInspector] public int id;

	private void Start()
	{
		Init();
	}

	/// <summary>
	/// Método redirecionada do Start para que possa ser chamado nas classes derivadas
	/// </summary>
	protected void Init()
	{
		text = GetComponent<TextMeshProUGUI>();

		if (text != null)
		{
			text.color = unselectedColor;
		}
	}

	/// <summary>
	/// Seta o estado de seleção do botão
	/// </summary>
	/// <param name="isSelected">estado de seleção (selecionado ou não selecionado)</param>
	public void SetSelected(bool isSelected)
	{
		direction = isSelected ? 1 : -1;
		menu.SetActive(isSelected);
	}

	public void Update()
	{
		if (direction != 0)
		{
			currentTime += Time.unscaledDeltaTime * direction;
			currentTime = Mathf.Clamp(currentTime, 0f, animationTime);
			float percent = currentTime / animationTime;
			if ((percent > 1f && direction == 1) || (percent < 0f && direction == -1))
			{
				direction = 0;
			}
			percent = Mathf.Clamp(percent, 0f, 1f);
			float value = selectionCurve.Evaluate(percent);

			ApplyAnimations(value);
		}
	}

	protected virtual void ApplyAnimations(float value)
	{
		text.color = new(
			Mathf.Lerp(unselectedColor.r, selectedColor.r, value),
			Mathf.Lerp(unselectedColor.g, selectedColor.g, value),
			Mathf.Lerp(unselectedColor.b, selectedColor.b, value),
			1f
		);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		transform.parent.GetComponent<NavbarController>().ForceSelect(id);
		CursorController.SetCursor(CursorSprite.Grab);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		transform.parent.GetComponent<NavbarController>().ForceSelect(-1);
		CursorController.SetCursor(CursorSprite.Default);
	}
}
