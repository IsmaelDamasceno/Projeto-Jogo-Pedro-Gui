using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawConnection : ConnectionComponent
{
    private Saw saw;

    public override void SetConnection(Transform connectionTrs)
    {
        saw.SetSpeed(connectionTrs.GetComponent<ChainRollerConnector>().systemSpeed);
	}

    public override void SetInterpolationValue(float value)
    {
    }

    public override void SetSignal(bool signalVal)
    {
        saw.direction = signalVal ? 1 : -1;
	}

    void Awake()
    {
        saw = GetComponent<Saw>();
    }

    void Update()
    {
        
    }
}
