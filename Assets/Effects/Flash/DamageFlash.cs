using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla e ativa o shader que pinta o objeto de uma cor s� quando sofre dano
/// </summary>
public class DamageFlash : MonoBehaviour, IFlash
{
    /// <summary>
    /// Cor do dano
    /// </summary>
	[SerializeField, ColorUsage(true, true)] private Color flashColor = Color.white;
    
    /// <summary>
    /// Curva que anima o n�vel do dano
    /// </summary>
    [SerializeField] private AnimationCurve flashCurve;

    /// <summary>
    /// Dura��o da anima��o de dano
    /// </summary>
    [SerializeField] private float flashTime = 0.25f;

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
        while(elapsedTime < flashTime)
        {
            // Atualiza o tempo
            elapsedTime += Time.deltaTime;

            // Pega o valor na curva de anima��o
            currentFlashAmount = flashCurve.Evaluate(elapsedTime / flashTime);
            // Aplica o valor na textura
            material.SetFloat("_Flash_Amount", currentFlashAmount);

            yield return null;
        }

        // Reseta o flash para 0 no fim da anima��o
		material.SetFloat("_Flash_Amount", 0f);
	}
}
