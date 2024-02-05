using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pressbutton : MonoBehaviour
{
	[Header("Color")]
	[SerializeField] protected Color unpressedColor;
	[SerializeField] protected Color pressedColor;

	[Header("Background Color")]
	[SerializeField] protected Color unpressedBackgroundColor;
	[SerializeField] protected Color pressedBackgroundColor;

	[Header("Animation controls")]
	[SerializeField] protected AnimationCurve pressCurve;
	[SerializeField] protected float animationTime;

	[Header("Events")]
	[SerializeField] protected UnityEvent onClickEvent;

	public bool pressed = false;

	protected Image image;
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
		image = GetComponent<Image>();
		text = Utils.SearchObjectWithComponent<TextMeshProUGUI>(transform, "Text");

		text.color = unpressedColor;
		image.color = unpressedBackgroundColor;
	}

	public void Press()
	{
		StartCoroutine(PressProgress(false));
	}
	public void VirtualPress()
	{
		StartCoroutine(PressProgress(true));
	}

	private IEnumerator PressProgress(bool isVirtual)
	{
		float time = 0f;
		pressed = true;
		if (!isVirtual)
		{
			Interact();
		}
		while (time < animationTime)
		{
			time += Time.deltaTime;
			float percent = time / animationTime;
			percent = Mathf.Clamp(percent, 0f, 1f);
			float value = pressCurve.Evaluate(percent);

			ApplyAnimations(value);

			yield return null;
		}
		pressed = false;
	}

	public void Update()
	{

	}

	protected virtual void ApplyAnimations(float value)
	{
		text.color = new(
				Mathf.Lerp(unpressedColor.r, pressedColor.r, value),
				Mathf.Lerp(unpressedColor.g, pressedColor.g, value),
				Mathf.Lerp(unpressedColor.b, pressedColor.b, value),
				1f
		);

		image.color = new(
			Mathf.Lerp(unpressedBackgroundColor.r, pressedBackgroundColor.r, value),
			Mathf.Lerp(unpressedBackgroundColor.g, pressedBackgroundColor.g, value),
			Mathf.Lerp(unpressedBackgroundColor.b, pressedBackgroundColor.b, value),
			Mathf.Lerp(unpressedBackgroundColor.a, pressedBackgroundColor.a, value)
		);
	}

	public virtual void Interact()
	{
		onClickEvent.Invoke();
	}
}
