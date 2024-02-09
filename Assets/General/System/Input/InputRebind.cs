using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputRebind : MonoBehaviour
{

    private static InputActionAsset inputActionAsset;

    void Awake()
    {
        inputActionAsset = Resources.Load<InputActionAsset>("Input/Input Actions");
        if (inputActionAsset == null)
        {
            Debug.LogError("Não foi possivel localizar \"InputActionAsset\"");
        }
    }

	public static void RebindAction(string actionName)
	{
		InputAction action = inputActionAsset.FindAction(actionName);

		if (action != null)
		{
			/*
			 // Start the rebind process
			var rebindOperation = action.PerformInteractiveRebinding()
				.WithControlsExcluding("<Mouse>*")
				.OnMatchWaitForAnother(0.1f)
				.OnComplete(operation =>
				{
					Debug.Log("CU");
					operation.Dispose();
				});

			rebindOperation.Start();
			 */

			var controls = action.controls;

			// Start the rebinding process for each control
			foreach (var control in controls)
			{
				Debug.Log($"device: {control.device}, name: {control.name}");
				/*
				// Perform interactive rebinding for the control
				var rebindOperation = control.PerformInteractiveRebinding()
					.OnMatchWaitForAnother(0.1f)
					.OnComplete(operation =>
					{
						// Once rebinding is complete, apply changes
						operation.Dispose();
					});

				// Start the rebinding operation
				rebindOperation.Start(); 
				*/
			}
		}
		else
		{
			Debug.LogError("Ação não encontrada: " + actionName);
		}
	}
}
