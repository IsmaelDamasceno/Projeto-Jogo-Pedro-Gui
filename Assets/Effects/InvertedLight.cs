using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InvertedLight : MonoBehaviour
{

    [SerializeField] private float intensity;

    private Light2D light;

    void Start()
    {
        light = GetComponent<Light2D>();
        light.intensity = intensity;
    }

    void Update()
    {
        
    }
}
