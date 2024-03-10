using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScoreLoader : MonoBehaviour
{

    [SerializeField] private GameObject heartPrefab;

    private TextMeshProUGUI nameField;
    private TextMeshProUGUI timeField;
    private GameObject healthParent;

    private void Init()
    {
		nameField = Utils.SearchObjectWithComponent<TextMeshProUGUI>(transform, "Name");
		timeField = Utils.SearchObjectWithComponent<TextMeshProUGUI>(transform, "Time");
		healthParent = Utils.SearchObjectIntransform(transform, "Health");
	}

    void Update()
    {
        
    }

    public void LoadScore(Score scoreToLoad)
    {
        if (nameField == null)
        {
            Init();
        }

        nameField.text = scoreToLoad.name;
		timeField.text = TimerController.ConvertSecondsToMMSSHH(scoreToLoad.time);

        foreach(Transform heart in healthParent.transform)
        {
            Destroy(heart.gameObject);
        }
        for(int i = 0; i < scoreToLoad.health; i++)
        {
            Instantiate(heartPrefab, healthParent.transform);
        }
    }
}
