using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    [SerializeField] private float explodeTime;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;

    private float curTime;

    [SerializeField] private LayerMask explosionMask;

    private SpriteRenderer maskSprite;

    void Start()
    {
        maskSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        StartCoroutine(WaitExplosion());
    }

    void Update()
    {
        curTime += Time.deltaTime / explodeTime;
		maskSprite.color = new Color(1f, 1f, 1f, curTime);
	}

    IEnumerator WaitExplosion()
    {
        yield return new WaitForSeconds(explodeTime);

        Collider2D[] hitList = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionMask);
        if (hitList.Length > 0)
        {
            foreach(Collider2D collider in hitList)
            {
                if (Vector2.Distance(collider.transform.position, transform.position) > explosionRadius)
                {
                    continue;
                }
                if (collider.TryGetComponent(out Rigidbody2D rb))
                {
					Vector2 direction = (collider.transform.position - transform.position).normalized;
					float percentage = 1f - (Vector2.Distance(collider.transform.position, transform.position) / explosionRadius);
					float force = percentage * explosionForce + rb.velocity.magnitude;

                    if (collider.TryGetComponent(out PlayerCore playerCore))
                    {
                        playerCore.stateMachine.ChangeState("Free");
                    }

                    rb.velocity = direction * force;
				}
            }
        }

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
#endif
}
