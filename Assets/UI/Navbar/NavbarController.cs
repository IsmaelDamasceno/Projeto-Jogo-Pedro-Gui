using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavbarController : MonoBehaviour
{
	[SerializeField] private bool isCursorBounded;

	private List<NavbarOption> options = new();
	private int selectedOption = 0;

	void Start()
    {
		#region Options
		int i = 0;
		foreach (Transform optionTrs in transform)
		{
			NavbarOption option = optionTrs.GetComponent<NavbarOption>();
			options.Add(option);
			option.id = i;
			i++;
		}
		options[selectedOption].SetSelected(true);
		#endregion

		InputListener.switchEvent.AddListener(SwitchListener);
	}

	private void SwitchListener(float value)
	{
		if (!enabled)
		{
			return;
		}

		int menuDirection = Math.Sign(value);
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
	}

	public void ForceSelect(int id)
	{
		if (!enabled)
		{
			return;
		}

		if (isCursorBounded)
		{
			if (selectedOption != -1)
			{
				options[selectedOption].SetSelected(false);
			}
			selectedOption = id;
			if (id != -1)
			{
				options[selectedOption].SetSelected(true);
			}
		}
		else if (id != -1)
		{
			options[selectedOption].SetSelected(false);
			selectedOption = id;
			options[selectedOption].SetSelected(true);
		}
	}


	void Update()
    {
        
    }
}
