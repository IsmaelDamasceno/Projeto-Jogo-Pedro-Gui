using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    private List<MenuOption> options = new();
    private int selectedOption = 0;

    void Start()
    {
        int i = 0;
        foreach(Transform optionTrs in transform)
        {
            MenuOption option = optionTrs.GetComponent<MenuOption>();
			options.Add(option);
            option.id = i;
            i++;
		}

        options[selectedOption].SetSelected(true);
    }

    void Update()
    {
        int menuUp = InputController.GetKeyDown("Menu Up")? 1: 0;
		int menuDown = InputController.GetKeyDown("Menu Down") ? 1 : 0;
        int menuDirection = menuDown - menuUp;
        if (menuDirection != 0)
        {
            CursorController.SetCursor(CursorSprite.Hidden);
            options[selectedOption].SetSelected(false);

            selectedOption += menuDirection;
            if (selectedOption < 0)
            {
                selectedOption = options.Count - 1;
            }
            else if (selectedOption >= options.Count)
            {
                selectedOption = 0;
            }

			options[selectedOption].SetSelected(true);
		}

        if (InputController.GetKeyDown("Menu Confirm") || (Cursor.visible && Input.GetMouseButtonDown(0)))
        {
            options[selectedOption].Interact();
        }
	}

    public void ForceSelect(int id)
    {
		options[selectedOption].SetSelected(false);
        selectedOption = id;
		options[selectedOption].SetSelected(true);
	}
}
