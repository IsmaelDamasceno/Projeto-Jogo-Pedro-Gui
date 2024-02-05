using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VirtualKeyboard : MenuController
{
    private VirtualKey[] keys;

    void Start()
    {
        keys = Utils.SearchObjectsWithComponent<VirtualKey>(transform, "Key Grid");
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
        }
    }
}
