using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsCancelHandler : MonoBehaviour
{
    void Start()
    {
		InputListener.cancelEvent.AddListener(Cancel);
	}

    public void Cancel()
    {
		MenuController menuController = GetComponent<MenuController>();
		menuController.CloseActivePanel();
	}
}
