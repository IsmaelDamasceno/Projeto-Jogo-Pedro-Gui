using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonConnection : ConnectionComponentInput
{
	[SerializeField] private Sprite upSprite;
	[SerializeField] private Sprite downSprite;

	private SpriteRenderer spriteRenderer;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		SendInput(true);
		spriteRenderer.sprite = downSprite;
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		SendInput(false);
		spriteRenderer.sprite = upSprite;
	}
}
