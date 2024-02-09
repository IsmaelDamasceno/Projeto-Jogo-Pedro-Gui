using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputButtonMapper : MonoBehaviour
{
	public static Sprite GetLabelFor(string button, string device)
	{
		Sprite buttonLabel = Resources.Load<Sprite>($"Controller Buttons/{device}/{button}");
		if (buttonLabel == null)
		{
			Debug.LogError($"Nenhum rótulo encontrado para botão {button} no dispositivo {device}");
		};
		return buttonLabel;
	}
}
