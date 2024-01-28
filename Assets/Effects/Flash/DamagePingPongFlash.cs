using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePingPongFlash : MonoBehaviour, IFlash
{
	/// <summary>
	/// Cor do dano
	/// </summary>
	[SerializeField, ColorUsage(true, true)] private Color flashColor = Color.white;

	/// <summary>
	/// Curva que anima o n�vel do dano
	/// </summary>
	[SerializeField] private AnimationCurve pingPongCurve;

	/// <summary>
	/// Dura��o da anima��o de dano
	/// </summary>
	[SerializeField] private float totalTime = 1f;

	[SerializeField] private float pingPongTime;

	/// <summary>
	/// Sprite renderer do GameObject
	/// </summary>
	private SpriteRenderer spriteRenderer;

	/// <summary>
	/// Material que aplica o flash visualmente
	/// </summary>
	private Material material;

	private float time;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		material = spriteRenderer.material;

		material.SetColor("_Flash_Color", flashColor);
	}

	/// <summary>
	/// Fun��o que dava ser executada para rodar a anima��o de flash
	/// </summary>
	public void Flash()
	{
		StopAllCoroutines();
		StartCoroutine(FlashDamage());
	}

	/// <summary>
	/// Coroutine que executa a anima��o
	/// </summary>
	/// <returns></returns>
	IEnumerator FlashDamage()
	{
		// N�vel de cor atual
		float currentFlashAmount = 0f;
		// Tempo que passou desde o in�cio da anima��o
		float elapsedTime = 0f;

		// Executa enquanto a anima��o n�o acaba
		while (elapsedTime < totalTime)
		{
			// Atualiza o tempo
			elapsedTime += Time.deltaTime;

			float time = elapsedTime % pingPongTime;

			// Pega o valor na curva de anima��o
			currentFlashAmount = pingPongCurve.Evaluate(time / pingPongTime);
			// Aplica o valor na textura
			material.SetFloat("_Flash_Amount", currentFlashAmount);

			yield return null;
		}

		// Reseta o flash para 0 no fim da anima��o
		material.SetFloat("_Flash_Amount", 0f);
	}
}
