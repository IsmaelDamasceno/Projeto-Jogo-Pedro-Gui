using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsável por executar animações e efeitos associados a bomba
/// </summary>
public class Bomb : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private AnimationCurve effectCurve;
    [SerializeField] private float radiusScale;
    [SerializeField] private float shakeScale;
    private float curTime;

	[Header("Explosion")]
	[SerializeField] private List<GameObject> explosionParticleObjects;
	[SerializeField] private float explosionEffectScale;

    [Header("Damage and Collision")]
    [SerializeField] private float explodeTime;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;

    [SerializeField] private LayerMask explosionMask;

    private Transform bombVisual;

    void Start()
    {
        bombVisual = transform.GetChild(0);
		StartCoroutine(WaitExplosion());
    }

    void Update()
    {
		#region Animação
		curTime += Time.deltaTime / explodeTime;

		float val = effectCurve.Evaluate(curTime);
		bombVisual.localScale = (1f + val * radiusScale) * new Vector3(1f, 1f, 1f);
		bombVisual.localPosition = shakeScale * val * Random.insideUnitCircle;
		#endregion
	}

	/// <summary>
	/// Explode a bomba após o tempo de espera acabar
	/// </summary>
	/// <returns></returns>
	IEnumerator WaitExplosion()
    {
		// Espera o tempo necessário
        yield return new WaitForSeconds(explodeTime);

		// Faz checagem de colisão para causar dano a objetos em volta
        CheckCollisions();

		// Cria as particulas de efeito
		foreach(GameObject particlePrefab in explosionParticleObjects)
		{
			Transform particleTrs = Instantiate(particlePrefab, transform.position, Quaternion.identity).transform;
			particleTrs.localScale = new Vector3(1f, 1f, 1f) * explosionEffectScale;
		}

		// Deleta a bomba
		Destroy(gameObject);
    }

	/// <summary>
	/// Checagem de colisão para causar dano a objetos próximos quando a bomba explode
	/// </summary>
    private void CheckCollisions()
    {
		// Procura objetos próximos e os coloca em uma lista
		Collider2D[] hitList = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionMask);

		if (hitList.Length > 0)
		{
			foreach (Collider2D collider in hitList)
			{
				if (Vector2.Distance(collider.transform.position, transform.position) > explosionRadius)
				{
					continue;
				}
				
				float percentage = 1f - (
					Vector2.Distance(collider.transform.position, transform.position) / explosionRadius);
				float force = percentage * explosionForce;

				if (collider.TryGetComponent(out IAttackable attackable))
				{
					attackable.SufferDamage(1, transform, default, force, .1f);
				}
			}
		}
	}

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
#endif
}
