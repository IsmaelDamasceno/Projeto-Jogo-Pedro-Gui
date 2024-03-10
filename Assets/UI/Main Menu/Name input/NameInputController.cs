using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SaveName();
		}
    }

    public void SaveName()
    {
		CheckpointSave.activePlayerName = inputField.text;
		CheckpointSave.Save();
		SceneManager.LoadScene("The End");
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
