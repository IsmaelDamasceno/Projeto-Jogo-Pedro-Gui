using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceChangeReaction : MonoBehaviour
{
    private GameObject keyboardObject;
    private GameObject dualShockObject;
    private GameObject gamepadObject;

    void Start()
    {
        keyboardObject = Utils.SearchObjectIntransform(transform, "Keyboard Object");
		dualShockObject = Utils.SearchObjectIntransform(transform, "DualShock Object");
		gamepadObject = Utils.SearchObjectIntransform(transform, "Gamepad Object");

		if (dualShockObject != null)
		{
			dualShockObject.SetActive(false);
		}
		if (gamepadObject != null)
		{
			gamepadObject.SetActive(false);
		}

		InputListener.controlChangeEvent.AddListener(React);
	}

    public void React(string newControlDevice)
    {
        keyboardObject.SetActive(false);
        if (dualShockObject != null)
        {
			dualShockObject.SetActive(false);
		}
        if (gamepadObject != null)
        {
			gamepadObject.SetActive(false);
		}

        switch(newControlDevice)
        {
            case "Keyboard":
                {
                    keyboardObject.SetActive(true);
                }break;
			case "DualShock":
				{
                    if (dualShockObject != null)
                    {
                        dualShockObject.SetActive(true);
                    }
                    else
                    {
                        gamepadObject.SetActive(true);
                    }
				}
				break;
			case "Gamepad":
				{
					if (gamepadObject != null)
					{
						gamepadObject.SetActive(true);
					}
					else
					{
						dualShockObject.SetActive(true);
					}
				}
				break;
		}
    }
}
