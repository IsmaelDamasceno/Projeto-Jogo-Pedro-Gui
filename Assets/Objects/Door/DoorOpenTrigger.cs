using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DoorOpenTrigger : MonoBehaviour, ILeverTrigger
{

    [SerializeField] private AnimationCurve openCurve;
    [SerializeField] private float animationTime;

    private BoxCollider2D collider;
    private float offset;

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        offset = collider.size.y;
    }

    public void Trigger()
    {
        StartCoroutine(OpenCoroutine());
    }

    IEnumerator OpenCoroutine()
    {
        float time = 0f;
        float initialY = transform.position.y;
        while(time < animationTime)
        {
            time += Time.deltaTime;
            time = Mathf.Clamp(time, 0f, animationTime);

            float percent = time / animationTime;
            float point = openCurve.Evaluate(percent);
            transform.position = new(
                transform.position.x, initialY + point * offset, transform.position.z    
            );

            yield return null;
        }
		transform.position = new(
			transform.position.x, initialY + offset, transform.position.z
		);

        collider.enabled = false;
        Destroy(gameObject);
	}
}
