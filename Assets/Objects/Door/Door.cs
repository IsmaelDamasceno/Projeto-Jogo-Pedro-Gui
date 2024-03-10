using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : ConnectionComponent
{

    [SerializeField] private float animationOffset;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float animationTime;

    private BoxCollider2D collider;
    private Transform doorTrs;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        doorTrs = transform.GetChild(0);
    }

    void Update()
    {

    }

    IEnumerator AnimationCoroutine(int direction)
    {
        float time = direction == 1? 0f: animationTime;
        while((time < animationTime && direction == 1) || (time > 0f && direction == -1))
        {
            time += Time.deltaTime * direction;
            time = Mathf.Clamp(time, 0f, animationTime);
            float point = curve.Evaluate((time / animationTime));
			doorTrs.localPosition = new(0f, point * animationOffset, 0f);

            yield return null;
        }
        if (time <= 0f)
        {
            collider.enabled = false;
        }
        else if (time >= animationTime)
        {
            collider.enabled = false;
        }
    }

    public override void SetSignal(bool signalVal)
    {
		StopAllCoroutines();
		StartCoroutine(AnimationCoroutine(signalVal? 1: -1));
	}

    public override void SetInterpolationValue(float value)
    {
    }

    public override void SetConnection(Transform connectionTrs)
    {
    }
}
