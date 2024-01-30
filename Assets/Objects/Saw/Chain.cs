using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Chain : MonoBehaviour
{

    [SerializeField] private bool invert;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
	}

    private void Update()
    {
        
    }

    /// <summary>
    /// Convert velocidade em metros por segundo para velocidade de animação
    /// </summary>
    /// <param name="speed">velocidade em metros por segundo para converter</param>
    public void SetSpeed(float speed)
    {
		// 2.0625f = ((3/16)*11)
        // pixels por frame
        // 3 / 16 = unidades por frame
        // 2.0625 = unidades por segundo
		animator.SetFloat("Speed Scale", (invert? -1f: 1f) * 2f / 2.0625f);
	}
}
