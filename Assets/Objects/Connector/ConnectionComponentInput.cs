using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionComponentInput : MonoBehaviour
{
    [SerializeField] private Connector connector;

    public void SendInput(bool inputVal)
    {
		connector.SetSignal(inputVal);
    }
}
