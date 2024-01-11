using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        curTime += Time.deltaTime / explodeTime;

        float val = effectCurve.Evaluate(curTime);
		bombVisual.localScale = (1f + val * radiusScale) * new Vector3(1f, 1f, 1f);
        bombVisual.localPosition = shakeScale * val * Random.insideUnitCircle;
	}

    IEnumerator WaitExplosion()
    {
        yield return new WaitForSeconds(explodeTime);

        CheckCollisions();

		foreach(GameObject particlePrefab in explosionParticleObjects)
		{
			Transform particleTrs = Instantiate(particlePrefab, transform.position, Quaternion.identity).transform;
			particleTrs.localScale = new Vector3(1f, 1f, 1f) * explosionEffectScale;
		}
		Destroy(gameObject);
    }


    private void CheckCollisions()
    {
		Collider2D[] hitList = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionMask);

		if (hitList.Length > 0)
		{
			foreach (Collider2D collider in hitList)
			{
				if (Vector2.Distance(collider.transform.position, transform.position) > explosionRadius)
				{
					continue;
				}
				/*
				Vector2 direction = (collider.transform.position - transform.position).normalized;
				float percentage = 1f - (Vector2.Distance(collider.transform.position, transform.position) / explosionRadius);
				float force = percentage * explosionForce + rb.velocity.magnitude;

				if (collider.TryGetComponent(out Player.PlayerCore playerCore))
				{
					playerCore.stateMachine.ChangeState("Free");
				}

				rb.velocity = direction * force; 
				*/

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
