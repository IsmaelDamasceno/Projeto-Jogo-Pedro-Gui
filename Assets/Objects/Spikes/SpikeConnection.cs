using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpikeConnection : ConnectionComponent
{

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

    public override void SetConnection(Transform connectionTrs)
    {

    }

    public override void SetInterpolationValue(float value)
    {
		light.intensity = value * maxIntensity;
	}

    public override void SetSignal(bool signalVal)
    {
		animator.SetBool("Active", signalVal);
	}
}
