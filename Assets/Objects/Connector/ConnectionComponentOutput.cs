using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionComponentOutput : MonoBehaviour
{
    public void SetSignal(bool signalVal)
    {
        Debug.Log($"received signal: {signalVal}");
    }
}
