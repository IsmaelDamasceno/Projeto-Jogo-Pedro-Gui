using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VirtualKeyboard : MenuController
{
    private VirtualKey[] keys;
    private VirtualKey[] controlKeys;
    private VirtualKey space;

    void Start()
    {
        keys = Utils.SearchObjectsWithComponent<VirtualKey>(transform, "Key Grid");
        controlKeys = Utils.SearchObjectsWithComponent<VirtualKey>(transform, "Virtual Keys");
        space = Utils.SearchObjectWithComponent<VirtualKey>(transform, "Space");
	}

    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach(VirtualKey key in keys)
            {
                if (key != null && Input.GetKeyDown(key.value.ToString()))
                {
                    key.GetComponent<Pressbutton>().VirtualPress();
					return;
                }
            }
            foreach(VirtualKey controlKey in controlKeys)
            {
                if (controlKey != null && Input.GetKeyDown(controlKey.keycodeValue))
                {
                    controlKey.GetComponent<Pressbutton>().VirtualPress();
                    return;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                space.GetComponent<Pressbutton>().VirtualPress();
            }
        }
    }
}
