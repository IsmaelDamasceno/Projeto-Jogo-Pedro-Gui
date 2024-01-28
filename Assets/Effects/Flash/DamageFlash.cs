using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla e ativa o shader que pinta o objeto de uma cor só quando sofre dano
/// </summary>
public class DamageFlash : MonoBehaviour, IFlash
{
    /// <summary>
    /// Cor do dano
    /// </summary>
	[SerializeField, ColorUsage(true, true)] private Color flashColor = Color.white;
    
    /// <summary>
    /// Curva que anima o nível do dano
    /// </summary>
    [SerializeField] private AnimationCurve flashCurve;

    /// <summary>
    /// Duração da animação de dano
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
    /// Função que dava ser executada para rodar a animação de flash
    /// </summary>
    public void Flash()
    {
        StopAllCoroutines();
		StartCoroutine(FlashDamage());
	}

    /// <summary>
    /// Coroutine que executa a animação
    /// </summary>
    /// <returns></returns>
    IEnumerator FlashDamage()
    {
        // Nível de cor atual
        float currentFlashAmount = 0f;
        // Tempo que passou desde o início da animação
        float elapsedTime = 0f;

        // Executa enquanto a animação não acaba
        while(elapsedTime < flashTime)
        {
            // Atualiza o tempo
            elapsedTime += Time.deltaTime;

            // Pega o valor na curva de animação
            currentFlashAmount = flashCurve.Evaluate(elapsedTime / flashTime);
            // Aplica o valor na textura
            material.SetFloat("_Flash_Amount", currentFlashAmount);

            yield return null;
        }

        // Reseta o flash para 0 no fim da animação
		material.SetFloat("_Flash_Amount", 0f);
	}
}
