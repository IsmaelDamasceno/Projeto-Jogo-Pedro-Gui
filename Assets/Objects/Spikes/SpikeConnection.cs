using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpikeConnection : ConnectionComponent
{
    [SerializeField] private float delay = 0f;
    [SerializeField] private bool invert = false;

    private Animator animator;
    private Material material;
    private Light2D light;

    private float maxIntensity;

    private void Start()
    {
        animator = GetComponent<Animator>();
        light = Utils.SearchObjectWithComponent<Light2D>(transform, "Light");
        maxIntensity = light.intensity;
        light.intensity = 0f;
	}
    
    private IEnumerator WaitForDelay(bool signalVal)
    {
        yield return new WaitForSeconds(delay);
		animator.SetBool("Active", signalVal);
	}

	public override void SetConnection(Transform connectionTrs)
	{

	}

	public override void SetInterpolationValue(float value)
    {
        if (invert)
        {
            value = 1f - value;
        }
		light.intensity = value * maxIntensity;
	}

    public override void SetSignal(bool signalVal)
    {
        StopAllCoroutines();
        signalVal = invert ? !signalVal : signalVal;
        StartCoroutine(WaitForDelay(signalVal));
	}
}
