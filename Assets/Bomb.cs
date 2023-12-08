using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    [SerializeField] private float explodeTime;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;

    private float curTime;

    [SerializeField] private LayerMask explosionMask;

    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(WaitExplosion());
    }

    void Update()
    {
        curTime += Time.deltaTime / explodeTime;
        sprite.color = new(curTime, curTime, curTime);
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

                    Debug.Log($"obj: {rb.gameObject.name}, direction:{direction}, percentage: {percentage}, old vel: {rb.velocity.magnitude}, force: {force}");

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
