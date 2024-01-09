using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
	[SerializeField, ColorUsage(true, true)] private Color flashColor = Color.white;
    
    [SerializeField] private AnimationCurve flashCurve;
    [SerializeField] private float flashTime = 0.25f;

    private SpriteRenderer spriteRenderer;
    private Material material;

    private Coroutine damageFlashCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;

		material.SetColor("_Flash_Color", flashColor);
    }

    public void Flash()
    {
        damageFlashCoroutine = StartCoroutine(FlashDamage());
    }

    IEnumerator FlashDamage()
    {
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while(elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = flashCurve.Evaluate(elapsedTime / flashTime);
            material.SetFloat("_Flash_Amount", currentFlashAmount);

            yield return null;
        }
		material.SetFloat("_Flash_Amount", 0f);
	}


    void Update()
    {
        
    }
}
