using UnityEngine;
using TMPro;

public class NameInputController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI charCounterText;

	private TMP_InputField inputField;

    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
	}

    void Update()
    {
        
    }

    public void OnTextChange()
    {
        if (inputField.text.Length > 15)
        {
			inputField.text = inputField.text[..15];
		}
        charCounterText.text = $"{inputField.text.Length:D2}/15";
	}
}
