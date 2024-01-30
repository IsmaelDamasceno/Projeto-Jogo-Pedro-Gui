using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ChainConnection : ConnectionComponent
{

    [SerializeField] private bool invert;

    private Animator animator;
    private float speed;
    private int direction;

    private void Awake()
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
		this.speed = speed / 2.0625f;
		UpdateValues();
	}

    public void SetDirection(int direction)
    {
        this.direction = direction;
        UpdateValues();
    }

    private void UpdateValues()
    {
		// 2.0625f = ((3/16)*11)
		// pixels por frame
		// 3 / 16 = unidades por frame
		// 2.0625 = unidades por segundo
		animator.SetFloat("Speed Scale", speed * direction);
	}

    public override void SetSignal(bool signalVal)
    {
        SetDirection(signalVal? 1: -1);
    }

    public override void SetInterpolationValue(float value)
    {
    }

    public override void SetConnection(Transform connectionTrs)
    {
        SetSpeed(connectionTrs.GetComponent<ChainRollerConnector>().systemSpeed);
	}
}
