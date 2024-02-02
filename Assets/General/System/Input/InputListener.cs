using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    private static InputListener instance;

    public static UnityEvent<Vector2> navigateEvent;
    public static UnityEvent confirmEvent;

	public static UnityEvent<float> moveEvent;
	public static UnityEvent attackEvent;
	public static UnityEvent<bool> jumpEvent;
	public static UnityEvent downdashEvent;

    private static PlayerInput playerInput;

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

			playerInput = GetComponent<PlayerInput>();

			instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError($"Cópia de InputListener encontrada, deletando {gameObject.name}");
            Destroy(gameObject);
        }
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
	public void OnAttack(InputValue value)
	{
		attackEvent.Invoke();
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

	public void OnConfirm()
	{
		confirmEvent.Invoke();
	}
	#endregion
}
