using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageParticles : MonoBehaviour
{

    private ParticleSystem partSystem;

    void Awake()
    {
        partSystem = GetComponent<ParticleSystem>();
    }

    public void SetColor(Color color)
    {
        ParticleSystem.MainModule main = partSystem.main;
        main.startColor = color;
    }

    void Update()
    {
        
    }
}
