using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionComponentInput : MonoBehaviour
{
    [SerializeField] private ConnectorInput inputConnector;

    public void SendInput(bool inputVal)
    {
        inputConnector.ReceiveInput(inputVal);
    }
}
