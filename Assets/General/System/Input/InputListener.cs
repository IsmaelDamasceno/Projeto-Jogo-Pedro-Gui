using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.PlayerInput;

public class InputListener : MonoBehaviour
{
    private static InputListener instance;

    public static UnityEvent<Vector2> navigateEvent;
    public static UnityEvent confirmEvent;

	public static UnityEvent<float> moveEvent;
	public static UnityEvent attackEvent;
	public static UnityEvent<bool> jumpEvent;
	public static UnityEvent downdashEvent;
	public static UnityEvent<string> controlChangeEvent;
	public static UnityEvent<float> switchEvent;
	public static UnityEvent pauseEvent;
	public static UnityEvent cancelEvent;

    public static PlayerInput playerInput;
	public static string activeDevice = "Keyboard";

	public static string activeMode = "UI";

    void Awake()
    {
        if (instance == null)
        {
			navigateEvent = new();
            confirmEvent = new();

			moveEvent = new();
			attackEvent = new();
			jumpEvent = new();
			downdashEvent = new();

			controlChangeEvent ??= new();
			switchEvent = new();
			pauseEvent = new();
			cancelEvent = new();

			playerInput = GetComponent<PlayerInput>();

			instance = this;

			TestDevice(GetComponent<PlayerInput>());
			SceneManager.sceneLoaded += SceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError($"Cópia de InputListener encontrada, deletando {gameObject.name}");
            Destroy(gameObject);
        }
    }
	
	private void SceneLoaded(Scene scene, LoadSceneMode mode)
	{
		TestDevice(GetComponent<PlayerInput>());
	}


    public static void SetInputMode(string mode)
    {
        playerInput.SwitchCurrentActionMap(mode);
    }

	#region Player
	public void OnMove(InputValue value)
	{
		moveEvent.Invoke(value.Get<float>());
	}
	public void OnAttack()
	{
		attackEvent.Invoke();
	}
	public void OnControlsChanged(PlayerInput input)
	{
		TestDevice(input);
	}
	private void TestDevice(PlayerInput input)
	{
		controlChangeEvent ??= new();
		foreach (InputDevice device in input.devices)
		{
			if (InputSystem.IsFirstLayoutBasedOnSecond(device.name, "DualShockGamepad"))
			{
				activeDevice = "DualShock";
			}
			else if (InputSystem.IsFirstLayoutBasedOnSecond(device.name, "Gamepad"))
			{
				activeDevice = "Gamepad";
			}
			else
			{
				activeDevice = "Keyboard";
			}
			controlChangeEvent.Invoke(activeDevice);
		}
	}

	public void OnJump(InputValue value)
	{
		jumpEvent.Invoke(value.isPressed);
	}

	public void OnDownDash()
	{
		downdashEvent.Invoke();
	}
	#endregion

	#region UI
	public void OnNavigate(InputValue value)
	{
		navigateEvent.Invoke(value.Get<Vector2>());
	}
	public void OnSwitch(InputValue value)
	{
		switchEvent.Invoke(value.Get<float>());
	}

	public void OnConfirm()
	{
		confirmEvent.Invoke();
	}
	#endregion

	public void OnPause()
	{
		pauseEvent.Invoke();
	}
	public void OnCancel()
	{
		cancelEvent.Invoke();
	}
}
