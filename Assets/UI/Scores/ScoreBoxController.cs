using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public struct Score
{
    public string name;
    public byte health;
    public float time;

    public Score(string name, byte health, float time)
    {
        this.name = name;
        this.health = health;
        this.time = time;
    }
}

public class ScoreBoxController : MonoBehaviour
{

    public static string[] fileNames;
    public static List<Score> scoreList = new();
    public static List<PlayerScoreLoader> scoreLoaderList = new();

    public static int selected = 0;
    private static ScoreBoxController instance;
    private static TextMeshProUGUI pageLabel;

    public static int page = 0;
    public static int totalPages;

    private void OnEnable()
    {
        LoadAllScoresFromMemory();
        LoadUI();
	}

    void Awake()
    {
        instance = this;

        scoreLoaderList = new();
        foreach(Transform child in transform)
        {
			scoreLoaderList.Add(child.GetComponent<PlayerScoreLoader>());
		}

        pageLabel = Utils.SearchObjectWithComponent<TextMeshProUGUI>(transform.parent, "Page");
		InputListener.navigateEvent.AddListener(NavigateListener);
	}

    private void NavigateListener(Vector2 direction)
    {
        int xDirection = Math.Sign(direction.x);
        if (xDirection != 0)
        {
            page += xDirection;
            if (page < 0)
            {
                page = totalPages-1;
            }
            else if (page >= totalPages)
            {
                page = 0;
            }

            pageLabel.text = $"Page: < {page+1}/{totalPages} >";
			LoadUI();
		}
    }

    void Update()
    {
        
    }

    public static void LoadAllScoresFromMemory()
    {
        scoreList = new();

        string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
		if (!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath);
		}
		fileNames = Directory.GetFiles(folderPath);

        foreach(string fileName in fileNames)
        {
            int PlayerNameStart = fileName.IndexOf("#")+1;
            int PlayerNameEnd = fileName.LastIndexOf("$")-1;

			string playerName = fileName[PlayerNameStart..PlayerNameEnd];
            float time = 0f;
            byte health = 0;

			using (FileStream fileStream = File.OpenRead(fileName))
			{
				using (BinaryReader reader = new BinaryReader(fileStream))
				{
					time = reader.ReadSingle();
					health = reader.ReadByte();
				}
			}

            Score newScore = new Score(playerName, health, time);
            scoreList.Add(newScore);
        }
        scoreList.Sort(new SortByTime());
        totalPages = (int)Mathf.Ceil(scoreList.Count / 5f);

		pageLabel.text = $"Page: < {page + 1}/{totalPages} >";
	}
    public static void LoadUI()
    {
        int itr = 0;
        foreach(PlayerScoreLoader scoreLoader in scoreLoaderList)
        {
            if (itr+page*5 >= scoreList.Count)
            {
                scoreLoader.gameObject.SetActive(false);
                itr++;
                continue;
            }
            else
            {
				scoreLoader.gameObject.SetActive(true);
			}
            scoreLoader.LoadScore(scoreList[itr+page*5]);
            itr++;
        }
    }

}

public class SortByTime : IComparer<Score>
{
	// Compare method to define custom sorting order
	public int Compare(Score x, Score y)
	{
		// Example: Sort by string length in ascending order
		if (x.time < y.time)
			return -1;
		else if (x.time > y.time)
			return 1;
		else
			return 0; // If lengths are equal, consider them equal
	}
}
