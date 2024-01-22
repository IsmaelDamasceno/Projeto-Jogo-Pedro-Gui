using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonConnection : ConnectionComponentInput
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		SendInput(true);
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		SendInput(false);
	}
}
